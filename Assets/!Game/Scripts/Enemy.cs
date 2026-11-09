using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy parameters")]
    public int health = 60;
    private int maxHealth;
    public int damage = 10;
    public float speed = 50f;
    public float attackCooldown = 1.5f;

    [Header("atack distance")]
    public float stopDistance = 40f;

    [Header("HP prefab")]
    public GameObject healthBarPrefab;
    private Image healthBarImage;

    private float nextAttackTime;

    public bool IsDead => health <= 0;

    private void Start()
    {
        if (maxHealth == 0) maxHealth = health;
        SpawnHealthBar();
    }

    public void InitializeEnemy(float statMultiplier)
    {
        health = Mathf.RoundToInt(health * statMultiplier);
        damage = Mathf.RoundToInt(damage * statMultiplier);
        maxHealth = health;

        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = 1f;
        }
    }

    private void SpawnHealthBar()
    {
        if (healthBarPrefab != null && healthBarImage == null)
        {
            GameObject barObj = Instantiate(healthBarPrefab, transform);
            healthBarImage = barObj.GetComponent<Image>();

            barObj.transform.localPosition = new Vector3(0, 40f, 0);
            if (healthBarImage != null) healthBarImage.fillAmount = 1f;
        }
    }

    private void Update()
    {
        Unit targetUnit = UnitManager.Instance.GetFirstUnitObject();

        if (targetUnit != null)
        {
            HandleMovement(targetUnit.transform.localPosition);
        }
        else
        {
            Castle castle = FindObjectOfType<Castle>();
            if (castle != null) HandleMovement(castle.transform.localPosition);
            else HandleMovement(UnitManager.Instance.baseSpawnPoint.localPosition);
        }
    }

    private void HandleMovement(Vector3 targetLocalPos)
    {
        Enemy frontEnemy = GetEnemyInFront();
        if (frontEnemy != null)
        {
            float distanceToFrontEnemy = Vector3.Distance(transform.localPosition, frontEnemy.transform.localPosition);
            if (distanceToFrontEnemy < frontEnemy.GetWidth()) return;
        }

        Vector3 flatTargetPos = new Vector3(targetLocalPos.x, transform.localPosition.y, transform.localPosition.z);
        float distanceToTarget = Vector3.Distance(transform.localPosition, flatTargetPos);

        if (distanceToTarget > stopDistance)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, flatTargetPos, speed * Time.deltaTime);
        }
        else
        {
            if (Time.time >= nextAttackTime)
            {
                ExecuteAttack();
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    private void ExecuteAttack()
    {
        Unit targetUnit = UnitManager.Instance.GetFirstUnitObject();
        if (targetUnit != null) targetUnit.TakeDamage(damage);
        else
        {
            Castle castle = FindObjectOfType<Castle>();
            if (castle != null) castle.TakeDamage(damage);
        }
    }

    private Enemy GetEnemyInFront()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        Enemy closestFront = null;
        float minDistance = float.MaxValue;

        foreach (Enemy e in allEnemies)
        {
            if (e == this || e.IsDead) continue;
            if (e.transform.localPosition.x < this.transform.localPosition.x)
            {
                float dist = this.transform.localPosition.x - e.transform.localPosition.x;
                if (dist < minDistance) { minDistance = dist; closestFront = e; }
            }
        }
        return closestFront;
    }

    public void RecalculateStopPosition() { }
    public float GetWidth() { return 35f; }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        health -= amount;

        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = Mathf.Clamp01((float)health / maxHealth);
        }

        if (health <= 0) Destroy(gameObject);
    }
}