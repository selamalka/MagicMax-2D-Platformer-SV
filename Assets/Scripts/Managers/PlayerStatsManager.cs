using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [field: SerializeField] public int CurrentLevel { get; private set; }
    [field: SerializeField] public float CurrentExp { get; private set; }
    [field: SerializeField] public float TargetExp { get; private set; }
    [field: SerializeField] public float TargetExpMultiplier { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public float MaxMana { get; private set; }
    [field: SerializeField] public float CurrentMana { get; private set; }
    [field: SerializeField] public int MeleeDamage { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentLevel = 1;
        CurrentHealth = MaxHealth;
    }

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

    public void SetCurrentMana(float value)
    {
        CurrentMana = value;
    }

    public void CheckExpToLevelUp()
    {
        if (CurrentExp >= TargetExp)
        {
            CurrentExp = 0;
            TargetExp *= TargetExpMultiplier;
            CurrentLevel++;
        }
    }

    public void GrantExp(float expValue)
    {
        SetCurrentExp(CurrentExp + expValue);
        CheckExpToLevelUp();
    }
}
