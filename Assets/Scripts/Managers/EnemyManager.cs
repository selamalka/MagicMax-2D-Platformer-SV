using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [field: Header("Imp")]
    [field: SerializeField] public float ImpMaxHealth { get; private set; }
    [field: SerializeField] public float ImpSpeed { get; private set; }
    [field: SerializeField] public float ImpExpValue { get; private set; }
    [field: SerializeField] public int ImpSoulValue { get; private set; }
    [field: SerializeField] public float ImpTimeBetweenProjectiles { get; private set; }
    [field: SerializeField] public float ImpDetectionRadius { get; private set; }
    [field: SerializeField] public GameObject ImpProjetilePrefab { get; private set; }

    [field: Header("Shadow Demon")]
    [field: SerializeField] public float ShadowDemonMaxHealth { get; private set; }
    [field: SerializeField] public float ShadowDemonSpeed { get; private set; }
    [field: SerializeField] public float ShadowDemonExpValue { get; private set; }
    [field: SerializeField] public int ShadowDemonSoulValue { get; private set; }
    [field: SerializeField] public float ShadowDemonTimeBetweenProjectiles { get; private set; }
    [field: SerializeField] public float ShadowDemonMaxDistanceFromPlayer { get; private set; }
    [field: SerializeField] public float ShadowDemonDetectionRadius { get; private set; }
    [field: SerializeField] public GameObject ShadowDemonProjetilePrefab { get; private set; }

    [field: Header("Behemoth")]
    [field: SerializeField] public float BehemothMaxHealth { get; private set; }
    [field: SerializeField] public float BehemothSpeed { get; private set; }
    [field: SerializeField] public float BehemothExpValue { get; private set; }
    [field: SerializeField] public int BehemothSoulValue { get; private set; }
    [field: SerializeField] public float BehemothTimeBetweenProjectiles { get; private set; }
    [field: SerializeField] public float BehemothDetectionRadius { get; private set; }
    [field: SerializeField] public GameObject BehemothProjetilePrefab { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
