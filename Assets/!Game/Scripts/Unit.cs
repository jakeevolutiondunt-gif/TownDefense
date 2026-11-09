using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Обязательно для работы с Image

public enum UnitType { Farmer, Knight, Archer, Spearman, Mage }

public class Unit : MonoBehaviour
{
    [Header("Unit parameters")]
    public UnitType type;
    public int maxHealth = 50;
    public int currentHealth;
    public int damage = 15;
    public int cost = 10;
    public float attackCooldown = 2f;

    [Header("HP prefab")]
    public GameObject healthBarPrefab;
    private Image healthBarImage;

    [Header("Farmer settings")]
    public int wheatProduction = 5;
    public float productionInterval = 3f;

    [HideInInspector]
    public Vector3 targetLocalPosition;
    public float moveSpeed = 100f;

    private bool isDead = false;

    public void SetInitialPosition(Vector3 startPos)
    {
        transform.localPosition = startPos;
        targetLocalPosition = startPos;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        SpawnHealthBar();

        if (type == UnitType.Farmer) StartCoroutine(ProduceWheatRoutine());
        else StartCoroutine(AttackRoutine());
    }

    private void Update()
    {
        if (transform.localPosition != targetLocalPosition)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetLocalPosition,
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void SpawnHealthBar()
    {
        if (healthBarPrefab != null)
        {
            GameObject barObj = Instantiate(healthBarPrefab, transform);
            healthBarImage = barObj.GetComponent<Image>();

            barObj.transform.localPosition = new Vector3(0, 40f, 0);

            if (healthBarImage != null) healthBarImage.fillAmount = 1f;
        }
    }

    private IEnumerator ProduceWheatRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(productionInterval);
            ResourceManager.Instance.AddWheat(wheatProduction);
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(attackCooldown);
            ExecuteAttack();
        }
    }

    private void ExecuteAttack()
    {
        if (type == UnitType.Knight)
        {
            Enemy target = GetFirstEnemy();
            if (target != null && IsInMeleeRange(target)) target.TakeDamage(damage);
        }
        else if (type == UnitType.Spearman)
        {
            Enemy target = GetFirstEnemy();
            if (target != null && IsInSpearmanRange(target)) target.TakeDamage(damage);
        }
        else if (type == UnitType.Archer)
        {
            Enemy target = GetFirstEnemy();
            if (target != null) target.TakeDamage(damage);
        }
        else if (type == UnitType.Mage)
        {
            AttackSplash(3, damage);
        }
    }

    private Enemy GetFirstEnemy() => FindObjectOfType<Enemy>();
    private bool IsInMeleeRange(Enemy enemy) => UnitManager.Instance.IsFirstUnit(this);
    private bool IsInSpearmanRange(Enemy enemy)
    {
        int index = UnitManager.Instance.GetUnitIndex(this);
        return index == 0 || index == 1;
    }

    private void AttackSplash(int maxTargets, int dmg)
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        int targetsHit = 0;
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (targetsHit >= maxTargets) break;
            allEnemies[i].TakeDamage(dmg);
            targetsHit++;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = Mathf.Clamp01((float)currentHealth / maxHealth);
        }

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
        UnitManager.Instance.RemoveUnitFromLine(this);
        Destroy(gameObject);
    }
}