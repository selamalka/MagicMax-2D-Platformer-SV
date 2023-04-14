using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth - damage);
        EventManager.OnPlayerGetHit?.Invoke();
        if (PlayerStatsManager.Instance.CurrentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
