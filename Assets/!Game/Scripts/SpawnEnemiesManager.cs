using System.Collections;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn settings")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform canvasTransform;

    [Header("Wave settings")]
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f;
    public float spawnDelay = 2.5f;

    [Header("Game start delay")]
    public float startDelay = 5f;

    [Header("Difficulty modificator")]
    public float difficultyScale = 1.05f;

    public TextMeshProUGUI waveTimerText;

    private int currentWaveNumber = 1;
    private float currentStatMultiplier = 1.0f;

    private void Start()
    {
        StartCoroutine(WaveSpawner());
    }

    private IEnumerator WaveSpawner()
    {
        float timeRemaining = startDelay;
        while (timeRemaining > 0)
        {
            UpdateTimerUI($"Первая волна через: {Mathf.CeilToInt(timeRemaining)} сек");
            yield return new WaitForSecondsRealtime(1f);
            timeRemaining -= 1f;
        }

        while (true)
        {
            UpdateTimerUI($"ВОЛНА №{currentWaveNumber}");
            Debug.Log($"ЗАПУСК ВОЛНЫ №{currentWaveNumber} (Сила врагов: x{currentStatMultiplier:F2})");

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy(i + 1);
                yield return new WaitForSecondsRealtime(spawnDelay);
            }

            while (CountActiveEnemies() > 0)
            {
                UpdateTimerUI($"Осталось врагов: {CountActiveEnemies()}");
                yield return new WaitForSeconds(0.5f);
            }

            Debug.Log($"Волна №{currentWaveNumber} ЗАЧИЩЕНА!");

            currentStatMultiplier *= difficultyScale;
            enemiesPerWave += 2;
            currentWaveNumber++;

            timeRemaining = timeBetweenWaves;
            while (timeRemaining > 0)
            {
                UpdateTimerUI($"Следующая волна через: {Mathf.CeilToInt(timeRemaining)} сек");
                yield return new WaitForSecondsRealtime(1f);
                timeRemaining -= 1f;
            }
        }
    }

    private void SpawnEnemy(int enemyIndex)
    {
        if (enemyPrefab == null || spawnPoint == null || canvasTransform == null) return;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, canvasTransform);
        newEnemy.transform.localScale = Vector3.one;

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.InitializeEnemy(currentStatMultiplier);
        }
    }

    private int CountActiveEnemies()
    {
        Enemy[] activeEnemies = FindObjectsOfType<Enemy>();
        int aliveCount = 0;
        foreach (Enemy enemy in activeEnemies)
            if (enemy != null && !enemy.IsDead) aliveCount++;
        return aliveCount;
    }

    private void UpdateTimerUI(string message)
    {
        if (waveTimerText != null)
        {
            waveTimerText.text = message;
        }
    }
}