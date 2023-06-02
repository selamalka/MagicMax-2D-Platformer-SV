using UnityEngine;

public class HealthPowerup : PowerupBase
{
    public override void PickupPowerup()
    {
        if (PlayerStatsManager.Instance.CurrentHealth < PlayerStatsManager.Instance.MaxHealth)
        {
            PlayerStatsManager.Instance.SetCurrentHealth(PlayerStatsManager.Instance.CurrentHealth + 1);
            UIManager.Instance.FillHealthPoint();
            Destroy(gameObject);
        }
        else
        {
            print("health is full");
            Destroy(gameObject);
        }
    }
}
