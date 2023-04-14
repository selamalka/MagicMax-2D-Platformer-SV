using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [field: SerializeField] public int CurrentLevel { get; private set; }
    [field: SerializeField] public float CurrentExp { get; private set; }
    [field: SerializeField] public float TargetExp { get; private set; }
    [field: SerializeField] public float TargetExpMultiplier { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int MaxMana { get; private set; }
    [field: SerializeField] public int MaxSouls { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public int CurrentMana { get; private set; }
    [field: SerializeField] public int CurrentSouls { get; private set; }
    [field: SerializeField] public float ManaFillTime { get; private set; }
    [field: SerializeField] public int MeleeDamage { get; private set; }

    private float ManaFillCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentLevel = 1;
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
        ManaFillCounter = ManaFillTime;
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
    }

    public void GrantExp(float expValue)
    {
        SetCurrentExp(CurrentExp + expValue);
        CheckExpToLevelUp();
    }

    public void GrantSouls(int soulValue)
    {
        CurrentSouls += soulValue;
        if (CurrentSouls >= MaxSouls)
        {
            CurrentMana += 1;
            UIManager.Instance.FillManaPoint();
            CurrentSouls = 0;
        }
    }

    public void HealthRegenHandler()
    {
        if (InputManager.Instance.IsFPressed() && CurrentMana > 0 && CurrentHealth < MaxHealth)
        {
            ManaFillCounter -= Time.deltaTime;

            if (ManaFillCounter <= 0)
            {
                RegenerateHealthPoint();
                ManaFillCounter = ManaFillTime;
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