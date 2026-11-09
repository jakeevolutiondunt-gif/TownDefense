using TMPro;
using System.Collections;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("Resource settings")]
    public int wheat = 50;

    [Header("Passive income")]
    public int passiveWheatAmount = 10;
    public float passiveWheatInterval = 2f;

    [Header("UI components")]
    public TextMeshProUGUI wheatText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        StartCoroutine(PassiveIncomeRoutine());
    }

    private IEnumerator PassiveIncomeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(passiveWheatInterval);
            AddWheat(passiveWheatAmount);
        }
    }

    public bool SpendWheat(int amount)
    {
        if (wheat >= amount)
        {
            wheat -= amount;
            UpdateUI();
            UnitManager.Instance.UpdateButtonsAvailability();
            return true;
        }
        return false;
    }

    public void AddWheat(int amount)
    {
        wheat += amount;
        UpdateUI();
        UnitManager.Instance.UpdateButtonsAvailability();
    }

    private void UpdateUI()
    {
        if (wheatText != null)
        {
            wheatText.text = $"Пшеница: {wheat}";
        }
    }
}
