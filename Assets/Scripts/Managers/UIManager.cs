using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Image[] healthPoints;
    [SerializeField] private Image[] manaPoints;
    [SerializeField] private Image[] soulPoints;
    [SerializeField] private Image expBarFull;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.OnPlayerGetHit += DecreaseHealthPoint;
        EventManager.OnEnemyDeath += UpdateExpBar;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerGetHit -= DecreaseHealthPoint;
        EventManager.OnEnemyDeath -= UpdateExpBar;
    }

    private void Start()
    {
        UpdateExpBar();
    }

    private void UpdateExpBar()
    {
        expBarFull.DOFillAmount(PlayerStatsManager.Instance.CurrentExp / PlayerStatsManager.Instance.TargetExp, 0.2f);
    }

    public void FillHealthPoint()
    {
        for (int i = 0; i < healthPoints.Length; i++)
        {
            if (healthPoints[i].color.a == 0)
            {
                healthPoints[i].DOFade(1, 0.2f);
                break;
            }
        }
    }
    private void DecreaseHealthPoint()
    {
        for (int i = PlayerStatsManager.Instance.CurrentHealth; i <= healthPoints.Length; i--)
        {
            if (healthPoints[i].color.a > 0)
            {
                healthPoints[i].DOFade(0, 0.2f);
                break;
            }
        }
    }

    public void FillManaPoint()
    {
        for (int i = 0; i < manaPoints.Length; i++)
        {
            if (manaPoints[i].color.a == 0)
            {
                manaPoints[i].DOFade(1, 0.2f);
                break;
            }
        }
    }
    public void DecreaseManaPoint()
    {
        for (int i = PlayerStatsManager.Instance.CurrentMana; i <= manaPoints.Length; i--)
        {
            if (manaPoints[i].color.a > 0)
            {
                manaPoints[i].DOFade(0, 0.2f);
                break;
            }
        }
    }

    public void FillSoulPoint()
    {
        for (int i = 0; i < soulPoints.Length; i++)
        {
            if (soulPoints[i].color.a == 0)
            {
                soulPoints[i].DOFade(1, 0.2f);
                break;
            }
        }
    }

    public void ResetSoulPoints()
    {
        foreach (var soulPoint in soulPoints)
        {
            soulPoint.DOFade(0, 0.2f);
        }
    }
}
