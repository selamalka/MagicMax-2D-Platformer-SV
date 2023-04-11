using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyType type;
    private float currentHealth;
    private float expValue;

    private void Start()
    {
        SetStatsByType();
    }

    private void SetStatsByType()
    {
        switch (type)
        {
            case EnemyType.None:
                break;

            case EnemyType.Imp:
                currentHealth = EnemyManager.Instance.ImpMaxHealth;
                expValue = EnemyManager.Instance.ImpExpValue;
                break;

            default:
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            PlayerStatsManager.Instance.GrantExp(expValue);
            EventManager.OnEnemyDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
