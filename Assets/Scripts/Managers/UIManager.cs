using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Image[] healthPoints;
    [SerializeField] private Image[] manaPoints;
    [SerializeField] private Image[] soulPoints;
    [SerializeField] private Image expBarFull;
    [SerializeField] private TextMeshProUGUI spellPointsValue;
    [SerializeField] private GameObject spellbookPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventManager.OnPlayerGetHit += DecreaseHealthPoint;
        EventManager.OnEnemyDeath += UpdateExpBar;
        EventManager.OnTPressed += ToggleSpellbook;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerGetHit -= DecreaseHealthPoint;
        EventManager.OnEnemyDeath -= UpdateExpBar;
        EventManager.OnTPressed -= ToggleSpellbook;
    }

    private void Start()
    {
        spellbookPanel.SetActive(false);
        UpdateExpBar();
        UpdateSpellPoints();
    }

    private void UpdateExpBar()
    {
        expBarFull.DOFillAmount(PlayerStatsManager.Instance.CurrentExp / PlayerStatsManager.Instance.TargetExp, 0.2f);
    }

    public void UpdateSpellPoints()
    {
        spellPointsValue.text = PlayerStatsManager.Instance.SpellPoints.ToString();
    }

    private void ToggleSpellbook()
    {
        if (!GameManager.Instance.IsPaused)
        {
            GameManager.Instance.PauseGame();
            spellbookPanel.gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.ResumeGame();
            spellbookPanel.gameObject.SetActive(false);
        }
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
            if (healthPoints[i] == null) break;
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
            if (manaPoints[i] == null) break;
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
            if (manaPoints[i] == null) break;
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
