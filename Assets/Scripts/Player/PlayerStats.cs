using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
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
