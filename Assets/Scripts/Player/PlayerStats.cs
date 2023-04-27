using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            Vector2 enemyDirection = collision.gameObject.transform.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PlayerController.Instance.Knockback(enemyDirection, 30, 20, 500);
        }
    }

    public void TakeDamage(int damage)
    {
        PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth - damage);
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
