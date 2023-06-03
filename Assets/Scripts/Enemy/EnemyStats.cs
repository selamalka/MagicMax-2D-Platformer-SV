using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyType type;
    [SerializeField] private float currentHealth;
    private float expValue;
    private int soulValue;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        switch (type)
        {
            case EnemyType.None:
                break;
            case EnemyType.Imp:
                AudioManager.Instance.PlayImpGetHit();
                break;
            case EnemyType.ShadowDemon:
                AudioManager.Instance.PlayShadowDemonGetHit();
                break;
            case EnemyType.Behemoth:
                AudioManager.Instance.PlayBehemothGetHit();
                break;
            case EnemyType.Zul:
                AudioManager.Instance.PlayZulGetHit();
                break;
            default:
                break;
        }

        AudioManager.Instance.PlayEnemyImpact();
        PlayerStatsManager.Instance.GrantSouls(soulValue);
        FXManager.Instance.FlashWhite(gameObject);

        if (currentHealth <= 0)
        {
            switch (type)
            {
                case EnemyType.None:
                    break;
                case EnemyType.Imp:
                    AudioManager.Instance.PlayImpDeathCry();
                    break;
                case EnemyType.ShadowDemon:
                    AudioManager.Instance.PlayShadowDemonDeathCry();
                    break;
                case EnemyType.Behemoth:
                    AudioManager.Instance.PlayBehemothDeathCry();
                    break;
                case EnemyType.Zul:
                    AudioManager.Instance.PlayZulDeathCry();
                    break;
                default:
                    break;
            }

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
