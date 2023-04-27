using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float invulnerableStartTime;
    [SerializeField] private bool isInvulnerable;
    private float invulnerableCounter;


    private void Update()
    {
        InvulnerableHandler();
    }

    private void InvulnerableHandler()
    {
        if (invulnerableCounter > 0)
        {
            invulnerableCounter -= Time.deltaTime;
        }
        else
        {
            isInvulnerable = false;
            invulnerableCounter = invulnerableStartTime;
        }
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

    private void TakeDamageOnEnemyCollision(Collision2D collision)
    {
        TakeDamage(1);
        Vector2 enemyDirection = collision.gameObject.transform.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerController.Instance.Knockback(enemyDirection, 30, 20, 500);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        isInvulnerable = true;
        invulnerableCounter = invulnerableStartTime;
        PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth - damage);
        FXManager.Instance.FlashWhite(gameObject);
        GameManager.Instance.PauseGameEffect(100);
        CameraShaker.Instance.Shake(3f, 0.15f);
        EventManager.OnPlayerGetHit?.Invoke();
        if (PlayerStatsManager.Instance.CurrentHealth <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.ResumeGame();
        }
    }
}
