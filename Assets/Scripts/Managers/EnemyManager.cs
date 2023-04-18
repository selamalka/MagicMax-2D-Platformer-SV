using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [field: Header("Imp")]
    [field: SerializeField] public float ImpMaxHealth { get; private set; }
    [field: SerializeField] public float ImpSpeed { get; private set; }
    [field: SerializeField] public float ImpExpValue { get; private set; }
    [field: SerializeField] public int ImpSoulValue { get; private set; }
    [field: SerializeField] public float ImpTimeBetweenShots { get; private set; }
    [field: SerializeField] public GameObject ImpProjetilePrefab { get; private set; }

    [field: Header("Shadow Demon")]
    [field: SerializeField] public float ShadowDemonMaxHealth { get; private set; }
    [field: SerializeField] public float ShadowDemonSpeed { get; private set; }
    [field: SerializeField] public float ShadowDemonExpValue { get; private set; }
    [field: SerializeField] public int ShadowDemonSoulValue { get; private set; }
    [field: SerializeField] public float ShadowDemonTimeBetweenShots { get; private set; }
    [field: SerializeField] public float ShadowDemonMaxDistanceFromPlayer { get; private set; }
    [field: SerializeField] public GameObject ShadowDemonProjetilePrefab { get; private set; }


    private void Awake()
    {
        Instance = this;
    }
}
