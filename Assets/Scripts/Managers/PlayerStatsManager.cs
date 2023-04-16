using System.Threading.Tasks;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [field: Header("Exp")]
    [field: SerializeField] public int CurrentLevel { get; private set; }
    [field: SerializeField] public float CurrentExp { get; private set; }
    [field: SerializeField] public float TargetExp { get; private set; }
    [field: SerializeField] public float TargetExpMultiplier { get; private set; }
    [field: SerializeField] public int SpellPoints { get; private set; }
    [field: Header("Health")]
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: Header("Mana")]
    [field: SerializeField] public int MaxMana { get; private set; }
    [field: SerializeField] public int CurrentMana { get; private set; }
    [field: SerializeField] public float ManaFillTime { get; private set; }
    [field: Header("Souls")]
    [field: SerializeField] public int MaxSouls { get; private set; }
    [field: SerializeField] public int CurrentSouls { get; private set; }
    [field: Header("Melee")]
    [field: SerializeField] public int MeleeDamage { get; private set; }

    private float manaFillCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentLevel = 1;
        SpellPoints = 0;
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
        manaFillCounter = ManaFillTime;
    }

    private void Update()
    {
        HealthRegenHandler();
    }

    #region Setters

    public void SetCurrentExp(float value)
    {
        CurrentExp = value;
    }

    public void SetTargetExp(float value)
    {
        TargetExp = value;
    }

    public void SetCurrentLevel(int value)
    {
        CurrentLevel = value;
    }

    public void SetCurrentHealth(int value)
    {
        CurrentHealth = value;
    }

    public void SetSpellPoints(int value)
    {
        SpellPoints = value;
    }

    #endregion

    public void UseMana(int manaCost)
    {
        for (int i = 1; i <= manaCost; i++)
        {
            CurrentMana--;
            UIManager.Instance.DecreaseManaPoint();
        }
    }

    public bool IsEnoughMana(int manaCost)
    {
        return CurrentMana - manaCost >= 0;
    }

    public void CheckExpToLevelUp()
    {
        if (CurrentExp >= TargetExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        CurrentExp = 0;
        TargetExp *= TargetExpMultiplier;
        CurrentLevel++;
        SpellPoints++;
        UIManager.Instance.UpdateSpellPoints();
    }

    public void GrantExp(float expValue)
    {
        SetCurrentExp(CurrentExp + expValue);
        CheckExpToLevelUp();
    }

    public async void GrantSouls(int soulValue)
    {
        if (CurrentMana == MaxMana) return;

        CurrentSouls += soulValue;
        UIManager.Instance.FillSoulPoint();
        if (CurrentSouls >= MaxSouls && CurrentMana < MaxMana)
        {
            await Task.Delay(300);
            CurrentSouls = 0;
            UIManager.Instance.ResetSoulPoints();
            CurrentMana += 1;
            UIManager.Instance.FillManaPoint();
        }
    }

    public void HealthRegenHandler()
    {
        if (InputManager.Instance.IsFPressed() && CurrentMana > 0 && CurrentHealth < MaxHealth)
        {
            manaFillCounter -= Time.deltaTime;

            if (manaFillCounter <= 0)
            {
                RegenerateHealthPoint();
                manaFillCounter = ManaFillTime;
            }
        }
    }

    private void RegenerateHealthPoint()
    {
        CurrentHealth++;
        UIManager.Instance.FillHealthPoint();
        CurrentMana--;
        UIManager.Instance.DecreaseManaPoint();
    }
}