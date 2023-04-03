using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    private float currentHealth;
    private float currentMana;


    private void Start()
    {
        currentHealth = PlayerStatsManager.Instance.MaxHealth;
        currentMana = PlayerStatsManager.Instance.MaxMana;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
}
