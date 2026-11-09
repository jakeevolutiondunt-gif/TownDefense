using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Castle : MonoBehaviour
{
    [Header("Castle settings")]
    public int maxHealth = 500;
    private int currentHealth;

    [Header("Interface")]
    public TextMeshProUGUI healthText;

    private List<Enemy> enemyQueue = new List<Enemy>();
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateCastleUI();
    }

    public float RegisterInQueue(Enemy enemy)
    {
        if (!enemyQueue.Contains(enemy))
        {
            enemyQueue.Add(enemy);
        }

        float castleRightEdge = (rectTransform.rect.width / 20);
        float totalOffset = 0f;

        for (int i = 0; i < enemyQueue.Count; i++)
        {
            if (enemyQueue[i] == enemy)
            {
                totalOffset += enemy.GetWidth() / 2;
                break;
            }
            totalOffset += enemyQueue[i].GetWidth() + 10f;
        }

        return castleRightEdge + totalOffset;
    }

    public void UnregisterFromQueue(Enemy enemy)
    {
        if (enemyQueue.Contains(enemy))
        {
            enemyQueue.Remove(enemy);
            UpdateQueuePositions();
        }
    }

    private void UpdateQueuePositions()
    {
        for (int i = 0; i < enemyQueue.Count; i++)
        {
            if (enemyQueue[i] != null)
            {
                enemyQueue[i].RecalculateStopPosition();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        currentHealth -= damage;
        UpdateCastleUI();
        if (currentHealth <= 0) GameOver();
    }

    private void UpdateCastleUI()
    {
        if (healthText != null) healthText.text = $"HP: {currentHealth}/{maxHealth}";
    }

    private void GameOver()
    {
        Debug.LogError("ЗАМОК РАЗРУШЕН!");
    }
}
