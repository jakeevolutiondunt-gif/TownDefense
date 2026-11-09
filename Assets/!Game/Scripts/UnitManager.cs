using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    [Header("Unit prefabs")]
    public GameObject farmerPrefab;
    public GameObject knightPrefab;
    public GameObject archerPrefab;
    public GameObject spearmanPrefab;
    public GameObject magePrefab;

    [Header("UI buttons")]
    public Button[] unitButtons = new Button[5];

    [Header("Cost on btn")]
    public TextMeshProUGUI[] priceTexts = new TextMeshProUGUI[5];

    [Header("Queue settings")]
    public float unitBuildTime = 3f;
    public TextMeshProUGUI queueVisualText;

    [Header("Column start")]
    public Transform baseSpawnPoint;
    public float distanceBetweenUnits = 40f;

    [Header("Farmer settings")]
    public float farmerOffsetFromCombat = 100f;
    public float distanceBetweenFarmers = 30f;

    private List<Unit> combatLine = new List<Unit>();
    private List<Unit> farmerLine = new List<Unit>();

    private Queue<int> purchaseQueue = new Queue<int>();
    private bool isBuilding = false;

    [Header("Progress visual")]
    public Image progressImage;

    private int[] unitCosts = { 10, 40, 70, 180, 300 };
    private string[] unitNames = { "Фермер", "Рыцарь", "Лучник", "Копейщик", "Маг" };

    private void Awake() { Instance = this; }
    private void Start() { InitButtonsText(); UpdateButtonsAvailability(); UpdateQueueUI(); }

    private void InitButtonsText()
    {
        for (int i = 0; i < priceTexts.Length; i++)
            if (priceTexts[i] != null) priceTexts[i].text = $"{unitCosts[i]}";
    }

    public void UpdateButtonsAvailability()
    {
        int currentWheat = ResourceManager.Instance.wheat;
        for (int i = 0; i < unitButtons.Length; i++)
        {
            if (unitButtons[i] == null) continue;
            bool canAfford = currentWheat >= unitCosts[i];
            unitButtons[i].interactable = canAfford;
            if (priceTexts[i] != null) priceTexts[i].color = canAfford ? Color.green : Color.red;
        }
    }

    public void BuyUnit(int unitId)
    {
        int index = unitId - 1;
        if (ResourceManager.Instance.SpendWheat(unitCosts[index]))
        {
            purchaseQueue.Enqueue(unitId);
            UpdateQueueUI();
            if (!isBuilding) StartCoroutine(ProcessQueueRoutine());
        }
    }

    private IEnumerator ProcessQueueRoutine()
    {
        isBuilding = true;
        while (purchaseQueue.Count > 0)
        {
            float elapsedTime = 0f;
            if (progressImage != null) progressImage.fillAmount = 1f;

            while (elapsedTime < unitBuildTime)
            {
                elapsedTime += Time.deltaTime;
                if (progressImage != null) progressImage.fillAmount = Mathf.Clamp01(1f - (elapsedTime / unitBuildTime));
                yield return null;
            }

            if (progressImage != null) progressImage.fillAmount = 0f;

            int nextUnitId = purchaseQueue.Dequeue();
            UpdateQueueUI();

            GameObject prefabToSpawn = null;
            bool isFarmer = (nextUnitId == 1);

            switch (nextUnitId)
            {
                case 1: prefabToSpawn = farmerPrefab; break;
                case 2: prefabToSpawn = knightPrefab; break;
                case 3: prefabToSpawn = archerPrefab; break;
                case 4: prefabToSpawn = spearmanPrefab; break;
                case 5: prefabToSpawn = magePrefab; break;
            }

            if (prefabToSpawn != null) SpawnUnitInLine(prefabToSpawn, isFarmer);
        }
        isBuilding = false;
    }

    private void SpawnUnitInLine(GameObject prefab, bool isFarmer)
    {
        GameObject newUnitObj = Instantiate(prefab, baseSpawnPoint.parent);
        Unit newUnit = newUnitObj.GetComponent<Unit>();

        float spawnY = baseSpawnPoint.localPosition.y;
        float spawnZ = baseSpawnPoint.localPosition.z;

        Vector3 spawnPos = new Vector3(baseSpawnPoint.localPosition.x - 500f, spawnY, spawnZ);
        newUnit.SetInitialPosition(spawnPos);

        if (isFarmer)
        {
            farmerLine.Add(newUnit);
        }
        else
        {
            combatLine.Add(newUnit);
        }

        RearrangeLine();
    }

    public void RearrangeLine()
    {
        float fixedY = baseSpawnPoint.localPosition.y;
        float fixedZ = baseSpawnPoint.localPosition.z;

        for (int i = 0; i < combatLine.Count; i++)
        {
            if (combatLine[i] == null) continue;

            Vector3 targetPos = new Vector3(baseSpawnPoint.localPosition.x - (i * distanceBetweenUnits), fixedY, fixedZ);
            combatLine[i].targetLocalPosition = targetPos;
            combatLine[i].gameObject.transform.localScale = Vector3.one;
        }

        float fixedFarmerStartX = 250f;

        for (int i = 0; i < farmerLine.Count; i++)
        {
            if (farmerLine[i] == null) continue;

            Vector3 targetPos = new Vector3(baseSpawnPoint.localPosition.x - (fixedFarmerStartX + (i * distanceBetweenFarmers)), fixedY, fixedZ);
            farmerLine[i].targetLocalPosition = targetPos;
            farmerLine[i].gameObject.transform.localScale = Vector3.one;
        }
    }

    public void RemoveUnitFromLine(Unit unit)
    {
        if (combatLine.Contains(unit)) combatLine.Remove(unit);
        if (farmerLine.Contains(unit)) farmerLine.Remove(unit);

        RearrangeLine();
    }

    public Unit GetFirstUnitObject()
    {
        if (combatLine.Count > 0 && combatLine[0] != null)
            return combatLine[0];

        if (farmerLine.Count > 0 && farmerLine[0] != null)
            return farmerLine[0];

        return null;
    }

    public bool IsFirstUnit(Unit unit)
    {
        return combatLine.Count > 0 && combatLine[0] == unit;
    }

    public int GetUnitIndex(Unit unit)
    {
        return combatLine.IndexOf(unit);
    }

    private void UpdateQueueUI()
    {
        if (queueVisualText == null) return;
        if (purchaseQueue.Count == 0) { queueVisualText.text = "Очередь производства пуста"; return; }
        string text = "В очереди: ";
        int count = 0;
        foreach (int unitId in purchaseQueue)
        {
            if (count > 3) { text += "..."; break; }
            text += unitNames[unitId - 1] + ", ";
            count++;
        }
        if (text.EndsWith(", ")) text = text.Substring(0, text.Length - 2);
        queueVisualText.text = text;
    }
}