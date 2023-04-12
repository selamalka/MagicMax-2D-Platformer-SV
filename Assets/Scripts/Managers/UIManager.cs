using DG.Tweening;
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

    private void Start()
    {
        UpdateManaBar(0);
        UpdateExpBar();
    }

    private void DecreaseHealthPoints()
    {
        for (int i = PlayerStatsManager.Instance.CurrentHealth - 1; i <= healthPoints.Length; i--)
        {
            if (healthPoints[i].color.a > 0)
            {
                healthPoints[i].DOFade(0, 0.2f);
                break;
            }
        }
    }

    private void UpdateManaBar(float notInUse)
    {
        manaBarFull.DOFillAmount(PlayerStatsManager.Instance.CurrentMana / PlayerStatsManager.Instance.MaxMana, 0.2f);
    }

    private void UpdateExpBar()
    {
        expBarFull.DOFillAmount(PlayerStatsManager.Instance.CurrentExp / PlayerStatsManager.Instance.TargetExp, 0.2f);
    }
}
