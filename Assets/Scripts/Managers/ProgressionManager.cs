using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;
    [field: SerializeField] public Progression Progression { get; private set; }
    [field: SerializeField] public bool IsTesting { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Progression.IsNewGame)
        {
            Progression.SetHealthPoints(5);
            UIManager.Instance.UpdateHealthPoints();

            Progression.SetManaPoints(0);
            UIManager.Instance.UpdateManaPoints();

            Progression.SetSoulPoints(0);
            UIManager.Instance.UpdateSoulPoints();

            Progression.SetTargetExp(10f);
            Progression.SetCurrentExpPoints(0);
            UIManager.Instance.UpdateExpBar();

            Progression.UnlockedSpellsList.Clear();
            Progression.SetSpellSlot1Info(null, null);
            Progression.SetSpellSlot2Info(null, null);
            Progression.SetLastCheckpoint(new Vector3(103, -14, 0));
            Progression.SetIsNewGame(false);
        }

        if (!IsTesting)
        {
            GameObject.Find("Player").transform.position = Progression.LastCheckpoint;
        }
        FindObjectOfType<PositionCameraToPlayer>().SetCameraToPlayer();
    }
}
