using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [field: SerializeField] public float ImpMaxHealth { get; private set; }
    [field: SerializeField] public float ImpSpeed { get; private set; }
    [field: SerializeField] public float ImpExpValue { get; private set; }
    [field: SerializeField] public int ImpSoulValue { get; private set; }
    [field: SerializeField] public float ImpTimeBetweenShots { get; private set; }
    [field: SerializeField] public float ImpTravelDistance { get; private set; }
    [field: SerializeField] public GameObject ImpProjetilePrefab { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
