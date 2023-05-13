using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject canvas;
    [SerializeField] private Image[] healthPoints;
    [SerializeField] private Image[] manaPoints;
    [SerializeField] private Image[] soulPoints;
    [SerializeField] private Image expBarFull;
    [SerializeField] private TextMeshProUGUI spellPointsValue;
    [SerializeField] private GameObject spellbookPanel;
    [SerializeField] private TextMeshProUGUI levelUpAnnouncement;

    private Image backgroundPanelImage;

    private void Awake()
    {
        Instance = this;        
        canvas.SetActive(true);
        backgroundPanelImage = GameObject.Find("Background Panel").GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventManager.OnPlayerGetHit += DecreaseHealthPoint;
        EventManager.OnEnemyDeath += UpdateExpBar;
        EventManager.OnTPressed += ToggleSpellbook;
        EventManager.OnPlayerLevelUp += UpdateSpellPoints;
        EventManager.OnPlayerLevelUp += AnnounceLevelUp;
        
    }

    private void OnDisable()
    {
        EventManager.OnPlayerGetHit -= DecreaseHealthPoint;
        EventManager.OnEnemyDeath -= UpdateExpBar;
        EventManager.OnTPressed -= ToggleSpellbook;
        EventManager.OnPlayerLevelUp -= UpdateSpellPoints;
        EventManager.OnPlayerLevelUp -= AnnounceLevelUp;
    }    

    private void Start()
    {
        FadeFromBlack(2);       
        spellbookPanel.SetActive(false);
        UpdateExpBar();
        UpdateSpellPoints();
    }

    private void Update()
    {
/*        if (backgroundPanelImage.color.a == 1)
        {
            FadeFromBlack(2);
        }*/
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

    public void FadeToBlack(float duration)
    {
        print("Fade To Black");
        backgroundPanelImage.DOColor(new Color(0, 0, 0, 1), duration);
    }
    public void FadeFromBlack(float duration)
    {
        print("Fade From Black");
        backgroundPanelImage.DOColor(new Color(0,0,0,0), duration);
    }

    private void UpdateExpBar()
    {
        expBarFull.DOFillAmount(PlayerStatsManager.Instance.CurrentExp / PlayerStatsManager.Instance.TargetExp, 0.2f);
    }
    public void UpdateSpellPoints()
    {
        spellPointsValue.text = PlayerStatsManager.Instance.SpellPoints.ToString();
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

    private void AnnounceLevelUp()
    {
        levelUpAnnouncement.gameObject.SetActive(true);
        levelUpAnnouncement.DOFade(1, 1f).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true);
        levelUpAnnouncement.transform.DOLocalMoveY(100, 2).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true)
            .OnComplete(() => levelUpAnnouncement.DOFade(0, 1).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true)
            .OnComplete(()=> levelUpAnnouncement.gameObject.SetActive(false)));
    }
}
