using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Image[] healthPoints;
    [SerializeField] private Image[] manaPoints;
    [SerializeField] private Image[] soulPoints;
    [SerializeField] private Image expBarFull;
    [SerializeField] private GameObject key;
    [SerializeField] private TextMeshProUGUI spellPointsValue;
    [SerializeField] private GameObject spellbookPanel;
    [SerializeField] private TextMeshProUGUI levelUpAnnouncement;
    [SerializeField] private TextMeshProUGUI tipText;

    private Image masterPanelImage;
    private Tween tipTween;

    private bool isPauseMenuOpen;
    private bool isSpellbookOpen;

    private void Awake()
    {
        Instance = this;
        canvas.SetActive(true);
        masterPanelImage = GameObject.Find("Master Panel").GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventManager.OnPlayerGetHit += DecreaseHealthPoint;
        EventManager.OnEnemyDeath += UpdateExpBar;
        EventManager.OnSPressed += ToggleSpellbook;
        EventManager.OnPlayerLevelUp += UpdateSpellPoints;
        EventManager.OnPlayerLevelUp += AnnounceLevelUp;
        EventManager.OnEscPressed += TogglePauseMenu;

    }

    private void OnDisable()
    {
        EventManager.OnPlayerGetHit -= DecreaseHealthPoint;
        EventManager.OnEnemyDeath -= UpdateExpBar;
        EventManager.OnSPressed -= ToggleSpellbook;
        EventManager.OnPlayerLevelUp -= UpdateSpellPoints;
        EventManager.OnPlayerLevelUp -= AnnounceLevelUp;
        EventManager.OnEscPressed -= TogglePauseMenu;
    }

    private void Start()
    {
        SetKeyDisplay(false);
        FadeFromBlack(2);
        spellbookPanel.SetActive(false);
        UpdateExpBar();
        UpdateSpellPoints();
    }

    public void SetKeyDisplay(bool value)
    {
        key.SetActive(value);
    }

    private void ToggleSpellbook()
    {
        if (!GameManager.Instance.IsPaused)
        {
            GameManager.Instance.PauseGame();
        }
        else if (GameManager.Instance.IsPaused && !isPauseMenuOpen)
        {
            GameManager.Instance.ResumeGame();
        }

        if (!spellbookPanel.gameObject.activeInHierarchy)
        {
            spellbookPanel.gameObject.SetActive(true);
            isSpellbookOpen = true;
            AudioManager.Instance.PlayOpenSpellbook();
        }
        else if (spellbookPanel.gameObject.activeInHierarchy)
        {
            spellbookPanel.gameObject.SetActive(false);
            isSpellbookOpen = false;
            AudioManager.Instance.PlayCloseSpellbook();
        }
    }

    public void TogglePauseMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) return;

        if (!GameManager.Instance.IsPaused)
        {
            GameManager.Instance.PauseGame();
        }
        else if (GameManager.Instance.IsPaused && !isSpellbookOpen)
        {
            GameManager.Instance.ResumeGame();
        }

        if (!pauseMenuPanel.gameObject.activeInHierarchy)
        {
            pauseMenuPanel.gameObject.SetActive(true);
            isPauseMenuOpen = true;
        }
        else if (pauseMenuPanel.gameObject.activeInHierarchy)
        {
            pauseMenuPanel.gameObject.SetActive(false);
            isPauseMenuOpen = false;
        }
    }

    public void FadeToBlack(float duration)
    {
        masterPanelImage.DOColor(new Color(0, 0, 0, 1), duration);
    }
    public void FadeFromBlack(float duration)
    {
        masterPanelImage.DOColor(new Color(0, 0, 0, 0), duration);
    }

    public void UpdateExpBar()
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
                healthPoints[i].DOFade(1, 0.2f).OnComplete(() => UpdateHealthPoints());
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
                healthPoints[i].DOFade(0, 0.2f).OnComplete(() => UpdateHealthPoints());
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
                manaPoints[i].DOFade(1, 0.2f).OnComplete(() => UpdateManaPoints());
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
                manaPoints[i].DOFade(0, 0.2f).OnComplete(() => UpdateManaPoints());
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
                soulPoints[i].DOFade(1, 0.2f).OnComplete(() => UpdateSoulPoints());
                break;
            }
        }
    }
    public void ResetSoulPoints()
    {
        foreach (var soulPoint in soulPoints)
        {
            soulPoint.DOFade(0, 0.2f).OnComplete(() => UpdateSoulPoints());
        }
    }

    public void UpdateHealthPoints()
    {
        foreach (var point in healthPoints)
        {
            point.color = new Color(point.color.r, point.color.g, point.color.b, 0);
        }

        for (int i = 0; i < PlayerStatsManager.Instance.CurrentHealth; i++)
        {
            healthPoints[i].color = new Color(healthPoints[i].color.r, healthPoints[i].color.g, healthPoints[i].color.b, 1);
        }
    }
    public void UpdateManaPoints()
    {
        foreach (var point in manaPoints)
        {
            point.color = new Color(point.color.r, point.color.g, point.color.b, 0);
        }

        for (int i = 0; i < PlayerStatsManager.Instance.CurrentMana; i++)
        {
            manaPoints[i].color = new Color(manaPoints[i].color.r, manaPoints[i].color.g, manaPoints[i].color.b, 1);
        }
    }
    public void UpdateSoulPoints()
    {
        foreach (var point in soulPoints)
        {
            point.color = new Color(point.color.r, point.color.g, point.color.b, 0);
        }

        for (int i = 0; i < PlayerStatsManager.Instance.CurrentSouls; i++)
        {
            soulPoints[i].color = new Color(soulPoints[i].color.r, soulPoints[i].color.g, soulPoints[i].color.b, 1);
        }
    }

    private void AnnounceLevelUp()
    {
        levelUpAnnouncement.gameObject.SetActive(true);
        levelUpAnnouncement.DOFade(1, 1f).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true);
        levelUpAnnouncement.transform.DOLocalMoveY(100, 2).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true)
            .OnComplete(() => levelUpAnnouncement.DOFade(0, 1).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true)
            .OnComplete(() => levelUpAnnouncement.gameObject.SetActive(false)));
    }

    public void ShowTip(string message)
    {
        tipText.gameObject.SetActive(true);
        tipText.text = message;
        tipTween = tipText.DOFade(1, 1f).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true);
    }
    public void HideTip()
    {
        tipTween = tipText.DOFade(0, 1f).SetLoops(0).SetEase(Ease.OutQuart).SetUpdate(true)
        .OnComplete(() => tipText.gameObject.SetActive(false));
    }
    public void KillTip()
    {
        tipTween.Kill();
    }
}