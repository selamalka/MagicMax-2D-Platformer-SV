using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool IsKeyCollected { get; private set; } = false;

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsKeyCollected)
        {
            UIManager.Instance.SetKeyDisplay(false);
            UIManager.Instance.FadeToBlack(2);
            await Task.Delay(2000);
            ProgressionManager.Instance.Progression.SetLastCheckpoint(new (103,-14,0));
            ProgressionManager.Instance.Progression.SetSpellPoints(PlayerStatsManager.Instance.SpellPoints);
            ProgressionManager.Instance.Progression.SetHealthPoints(PlayerStatsManager.Instance.CurrentHealth);
            ProgressionManager.Instance.Progression.SetManaPoints(PlayerStatsManager.Instance.CurrentMana);
            ProgressionManager.Instance.Progression.SetSoulPoints(PlayerStatsManager.Instance.CurrentSouls);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            print("You require a key");
        }
    }

    public void SetIsKeyCollected(bool value)
    {
        IsKeyCollected = value;
    }
}
