using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;
    [field: SerializeField] public Progression Progression { get; private set; }
    [SerializeField] private bool isTesting;

    private void Awake()
    {
        Instance = this;

        if (Progression.IsNewGame)
        {
            PlayerCombat.Instance.ClearSpellSlots();
            Progression.UnlockedSpellsList.Clear();
            Progression.SetLastCheckpoint(new Vector3(103,-14,0));
            Progression.SetIsNewGame(false);
        }
    }

    private void Start()
    {
        if (!isTesting)
        {
            GameObject.Find("Player").transform.position = Progression.LastCheckpoint; 
        }
        FindObjectOfType<PositonCameraToPlayer>().SetCameraToPlayer();
    }
}
