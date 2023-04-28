using UnityEngine;

public class ManaPowerup : PowerupAbstract
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
