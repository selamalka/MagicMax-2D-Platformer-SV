using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [field: SerializeField] public GameObject ImpPrefab { get; private set; }
    [field: SerializeField] public GameObject ShadowDemonPrefab { get; private set; }
    [field: SerializeField] public GameObject BehemothPrefab { get; private set; }

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
    [field: SerializeField] public float BehemothTimeBetweenMelee { get; private set; }
    [field: SerializeField] public float BehemothRangedDetectionRadius { get; private set; }
    [field: SerializeField] public float BehemothMeleeDetectionRadius { get; private set; }
    [field: SerializeField] public GameObject BehemothProjetilePrefab { get; private set; }

    [field: Header("Zul")]
    [field: SerializeField] public float ZulMaxHealth { get; private set; }
    [field: SerializeField] public float ZulSpeed { get; private set; }
    [field: SerializeField] public int ZulSoulValue { get; private set; }
    [field: SerializeField] public float ZulTimeBetweenProjectiles { get; private set; }
    [field: SerializeField] public GameObject ZulProjectilePrefab { get; private set; }
    [field: SerializeField] public Transform[] ZulSpawnPoints { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
