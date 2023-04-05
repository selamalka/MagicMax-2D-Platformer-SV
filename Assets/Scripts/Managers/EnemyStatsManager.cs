using UnityEngine;

public class EnemyStatsManager : MonoBehaviour
{
    public static EnemyStatsManager Instance;

    [field: SerializeField] public float ImpMaxHealth { get; private set; }
    [field: SerializeField] public float ImpSpeed { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
