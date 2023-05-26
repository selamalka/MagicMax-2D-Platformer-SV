using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;
    [field: SerializeField] public Progression Progression { get; private set; }
    [SerializeField] private bool isTesting;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Progression.IsNewGame)
        {
            Progression.UnlockedSpellsList.Clear();
            Progression.SetSpellSlot1Info(null, null);
            Progression.SetSpellSlot2Info(null, null);
            Progression.SetHealthPoints(5);
            Progression.SetManaPoints(0);
            Progression.SetLastCheckpoint(new Vector3(103, -14, 0));
            Progression.SetIsNewGame(false);
        }

        if (!isTesting)
        {
            GameObject.Find("Player").transform.position = Progression.LastCheckpoint;
        }
        FindObjectOfType<PositonCameraToPlayer>().SetCameraToPlayer();
    }
}
