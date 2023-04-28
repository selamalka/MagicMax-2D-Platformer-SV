using UnityEngine;

public class HealthPowerup : PowerupAbstract
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivatePowerup();
        }
    }

    public override void ActivatePowerup()
    {
        if (PlayerStatsManager.Instance.CurrentHealth < PlayerStatsManager.Instance.MaxHealth)
        {
            PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth + 1);
            UIManager.Instance.FillHealthPoint();
            Destroy(gameObject);
        }
        else
        {
            print("heatlh is full");
            Destroy(gameObject);
        }
    }
}
