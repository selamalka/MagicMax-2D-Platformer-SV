using System.Threading.Tasks;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
            Vector2 enemyDirection = collision.gameObject.transform.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            PlayerController.Instance.SetIsControllable(false);
            PlayerController.Instance.PushPlayerAgainstDirection(enemyDirection, 20);
            await Task.Delay(250);
            PlayerController.Instance.SetIsControllable(true);
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
