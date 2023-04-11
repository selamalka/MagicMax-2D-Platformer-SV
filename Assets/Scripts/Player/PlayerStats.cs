using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damage)
    {
        EventManager.OnPlayerGetHit?.Invoke();
        PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth - damage);
        if (PlayerStatsManager.Instance.CurrentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
