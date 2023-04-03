using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float MaxMana { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
