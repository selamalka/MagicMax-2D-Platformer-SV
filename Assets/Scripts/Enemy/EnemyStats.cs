using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyType type;
    [SerializeField] private float currentHealth;
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

            case EnemyType.Zul:
                currentHealth = EnemyManager.Instance.ZulMaxHealth;
                break;

            default:
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        PlayerStatsManager.Instance.GrantSouls(soulValue);
        FXManager.Instance.FlashWhite(gameObject);

        if (currentHealth <= 0)
        {
            PlayerStatsManager.Instance.GrantExp(expValue);            
            EventManager.OnEnemyDeath?.Invoke();
            if (FXManager.Instance.EnemyDeathShakePreset != null)
            {
                FXManager.Instance.CameraShaker.Shake(FXManager.Instance.EnemyDeathShakePreset); 
            }
            FXManager.Instance.PauseGameEffect(60);
            Destroy(gameObject, 0.1f);
            PowerupManager.Instance.DropRandomPowerup(transform, PowerupManager.Instance.DropChance);
        }
    }
}
