using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public static PlayerStats Instance;

    [field: SerializeField] public float InvulnerableStartTime { get; private set; }
    [SerializeField] private bool isInvulnerable;
    private float invulnerableCounter;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        InvulnerableHandler();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!isInvulnerable)
            {
                TakeDamageOnEnemyCollision(collision);
            }
        }
    }

    private void InvulnerableHandler()
    {
        if (!isInvulnerable) return;

        Physics2D.IgnoreLayerCollision(7, 8);

        if (invulnerableCounter > 0)
        {
            invulnerableCounter -= Time.deltaTime;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(7, 8, false);
            isInvulnerable = false;
            invulnerableCounter = InvulnerableStartTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        isInvulnerable = true;
        invulnerableCounter = InvulnerableStartTime;
        PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth - damage);
        FXManager.Instance.FlashWhite(gameObject);
        FXManager.Instance.PauseGameEffect(100);
        CameraShaker.Instance.Shake(3f, 0.15f);
        EventManager.OnPlayerGetHit?.Invoke();
        if (PlayerStatsManager.Instance.CurrentHealth <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.ResumeGame();
        }
    }

    private void TakeDamageOnEnemyCollision(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BehemothAI>() == null)
        {
            TakeDamage(1);
        }
        Vector2 enemyDirection = collision.gameObject.transform.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerController.Instance.Knockback(enemyDirection, 30, 20, 500);
    }
}
