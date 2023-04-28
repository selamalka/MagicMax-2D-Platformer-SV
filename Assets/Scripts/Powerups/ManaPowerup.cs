using UnityEngine;

public class ManaPowerup : PowerupBase
{
    public override void ActivatePowerup()
    {
        if (PlayerStatsManager.Instance.CurrentMana < PlayerStatsManager.Instance.MaxMana)
        {
            PlayerStatsManager.Instance.AddMana(1);
            Destroy(gameObject);
        }
        else
        {
            print("mana is full");
            Destroy(gameObject);
        }
    }
}
