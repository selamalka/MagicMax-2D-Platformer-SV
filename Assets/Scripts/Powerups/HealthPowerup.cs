using UnityEngine;

public class HealthPowerup : PowerupBase
{
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
