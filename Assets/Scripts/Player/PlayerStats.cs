using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    private int currentHealth;
    private float currentMana;


    private void Start()
    {
        currentHealth = PlayerStatsManager.Instance.MaxHealth;
        currentMana = PlayerStatsManager.Instance.MaxMana;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        EventManager.OnPlayerGetHit?.Invoke();
        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
