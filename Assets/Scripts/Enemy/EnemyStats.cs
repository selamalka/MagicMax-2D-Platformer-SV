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

            case EnemyType.Behemoth:
                currentHealth = EnemyManager.Instance.BehemothMaxHealth;
                expValue = EnemyManager.Instance.BehemothExpValue;
                soulValue = EnemyManager.Instance.BehemothSoulValue;
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
            GameManager.Instance.PauseGameEffect(200);
            CameraShaker.Instance.Shake(7f, 0.2f);
            Destroy(gameObject, 0.15f);
        }
    }
}
