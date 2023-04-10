using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [field: SerializeField] public int CurrentLevel { get; private set; }
    [field: SerializeField] public float CurrentExp { get; private set; }
    [field: SerializeField] public float TargetExp { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float MaxMana { get; private set; }
    [field: SerializeField] public float MeleeDamage { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentLevel = 1;
    }


}
