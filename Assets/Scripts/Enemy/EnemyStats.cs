using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyType type;
    private float currentHealth;
    private float expValue;
    private int soulValue;

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
                soulValue = EnemyManager.Instance.ImpSoulValue;
                break;

            case EnemyType.ShadowDemon:
                currentHealth = EnemyManager.Instance.ShadowDemonMaxHealth;
                expValue = EnemyManager.Instance.ShadowDemonExpValue;
                soulValue = EnemyManager.Instance.ShadowDemonSoulValue;
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
            PlayerStatsManager.Instance.GrantSouls(soulValue);
            EventManager.OnEnemyDeath?.Invoke();
            StartCoroutine(GameManager.Instance.PauseGameEffect(GameManager.Instance.PauseTime));
            Destroy(gameObject, 0.15f);
        }
    }
}
