using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] healthPoints;
    [SerializeField] private Image manaBarFull;
    [SerializeField] private Image expBarFull;

    private void OnEnable()
    {
        EventManager.OnPlayerUseMana += UpdateManaBar;
        EventManager.OnEnemyDeath += UpdateExpBar;
        EventManager.OnPlayerGetHit += DecreaseHealthPoints;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerGetHit -= DecreaseHealthPoints;
        EventManager.OnPlayerUseMana -= UpdateManaBar;
        EventManager.OnEnemyDeath -= UpdateExpBar;
    }

    private void DecreaseHealthPoints()
    {
        for (int i = PlayerStatsManager.Instance.CurrentHealth - 1; i <= healthPoints.Length; i--)
        {
            if (healthPoints[i].enabled)
            {
                healthPoints[i].enabled = false;
                break;
            }
        }
    }

    private void UpdateManaBar()
    {
        manaBarFull.fillAmount = PlayerStatsManager.Instance.CurrentMana / PlayerStatsManager.Instance.MaxMana;
    }

    private void UpdateExpBar()
    {
        expBarFull.fillAmount = PlayerStatsManager.Instance.CurrentExp / PlayerStatsManager.Instance.TargetExp;
    }

}
