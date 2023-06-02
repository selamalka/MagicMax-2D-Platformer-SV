using System.Threading.Tasks;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [field: Header("Exp")]
    //[field: SerializeField] public int CurrentLevel { get; private set; }
    [field: SerializeField] public float CurrentExp { get; private set; }
    [field: SerializeField] public float TargetExp { get; private set; }
    [field: SerializeField] public float TargetExpMultiplier { get; private set; }
    [field: SerializeField] public int SpellPoints { get; private set; }
    [field: Header("Health")]
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public float HealthRegenFillTime { get; private set; }
    [field: Header("Mana")]
    [field: SerializeField] public int MaxMana { get; private set; }
    [field: SerializeField] public int CurrentMana { get; private set; }
    [field: Header("Souls")]
    [field: SerializeField] public int MaxSouls { get; private set; }
    [field: SerializeField] public int CurrentSouls { get; private set; }
    [field: Header("Melee")]
    [field: SerializeField] public int MeleeDamage { get; private set; }

    private float healthRegenFillCounter;
    private bool isChargingHealthActivated = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //CurrentLevel = 1;

        CurrentHealth = ProgressionManager.Instance.Progression.HealthPoints;
        CurrentMana = ProgressionManager.Instance.Progression.ManaPoints;
        CurrentSouls = ProgressionManager.Instance.Progression.SoulPoints;
        TargetExp = ProgressionManager.Instance.Progression.TargetExp;
        CurrentExp = ProgressionManager.Instance.Progression.ExpPoints;


        SpellPoints = ProgressionManager.Instance.Progression.SpellPoints;

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

    /*    public void SetCurrentLevel(int value)
        {
            CurrentLevel = value;
        }*/

    public void SetCurrentHealth(int value)
    {
        CurrentHealth = value;
    }
    public void SetCurrentMana(int value)
    {
        CurrentMana = value;
    }
    public void SetCurrentSouls(int value)
    {
        CurrentSouls = value;
    }

    public void SetSpellPoints(int value)
    {
        SpellPoints = value;
    }

    #endregion

    private void RegenerateHealthPoint()
    {
        CurrentHealth++;
        UIManager.Instance.FillHealthPoint();
        CurrentMana--;
        UIManager.Instance.DecreaseManaPoint();
    }
    public void HealthRegenHandler()
    {
        if (Input.GetKey(KeyCode.F) && CurrentMana > 0 && CurrentHealth < MaxHealth)
        {
            PlayerController.Instance.SetIsControllable(false);

            if (!isChargingHealthActivated)
            {
                PlayerController.Instance.Animator.SetTrigger("triggerChargeHealth");
                PlayerController.Instance.Animator.SetBool("isChargingHealth", true);
                isChargingHealthActivated = true;
            }

            if (PlayerController.Instance.Rb != null)
            {
                PlayerController.Instance.Rb.velocity = new Vector2(0, PlayerController.Instance.Rb.velocity.y);
            }

            healthRegenFillCounter -= Time.deltaTime;

            if (healthRegenFillCounter <= 0)
            {
                RegenerateHealthPoint();
                healthRegenFillCounter = HealthRegenFillTime;
            }
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            PlayerController.Instance.SetIsControllable(true);
            PlayerController.Instance.Animator.SetBool("isChargingHealth", false);
            isChargingHealthActivated = false;
            healthRegenFillCounter = HealthRegenFillTime;
        }
    }

    public void AddMana(int manaValue)
    {
        if (CurrentMana == MaxMana) return;

        for (int i = 1; i <= manaValue; i++)
        {
            CurrentMana++;
            UIManager.Instance.FillManaPoint();
            ProgressionManager.Instance.Progression.SetManaPoints(CurrentMana);
        }
    }
    public void UseMana(int manaCost)
    {
        if (CurrentMana == 0) return;

        for (int i = 1; i <= manaCost; i++)
        {
            CurrentMana--;
            UIManager.Instance.DecreaseManaPoint();
            ProgressionManager.Instance.Progression.SetManaPoints(CurrentMana);
        }
    }
    public bool IsEnoughMana(int manaCost)
    {
        return CurrentMana - manaCost >= 0;
    }
    public async void GrantSouls(int soulValue)
    {
        if (CurrentMana >= MaxMana)
        {
            CurrentMana = MaxMana;
            return;
        }

        if (CurrentSouls < MaxSouls)
        {
            CurrentSouls += soulValue;
            UIManager.Instance.FillSoulPoint();
            ProgressionManager.Instance.Progression.SetSoulPoints(CurrentSouls);

            if (CurrentSouls >= MaxSouls)
            {
                await Task.Delay(200);
                CurrentSouls = 0;
                UIManager.Instance.ResetSoulPoints();

                if (CurrentMana < MaxMana)
                {
                    CurrentMana++;
                    UIManager.Instance.FillManaPoint();
                }
            }
        }
    }

    private void LevelUp()
    {
        CurrentExp = 0;
        TargetExp *= TargetExpMultiplier;
        ProgressionManager.Instance.Progression.SetTargetExp(TargetExp);
        SpellPoints++;
        //CurrentLevel++;
        //UIManager.Instance.UpdateSpellPoints();
    }
    public void CheckExpToLevelUp()
    {
        if (CurrentExp >= TargetExp)
        {
            LevelUp();
            EventManager.OnPlayerLevelUp?.Invoke();
        }
    }
    public void GrantExp(float expValue)
    {
        SetCurrentExp(CurrentExp + expValue);
        ProgressionManager.Instance.Progression.SetCurrentExpPoints(CurrentExp);
        CheckExpToLevelUp();
    }
}