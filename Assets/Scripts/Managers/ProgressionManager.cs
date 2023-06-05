using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;
    [field: SerializeField] public Progression Progression { get; private set; }
    [field: SerializeField] public bool IsTesting { get; private set; }
    [field: SerializeField] public bool IsCheckpointSaved { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (Progression.IsNewGame)
        {
            Progression.SetSpellSlot1Info(null, null);
            Progression.SetSpellSlot2Info(null, null); 
        }
    }

    private void Start()
    {
        if (Progression.IsNewGame)
        {
            Progression.SetHealthPoints(5);
            Progression.SetManaPoints(0);
            Progression.SetSoulPoints(0);
            Progression.SetTargetExp(10f);
            Progression.SetCurrentExpPoints(0);

            PlayerStatsManager.Instance.SetCurrentHealth(5);
            PlayerStatsManager.Instance.SetCurrentMana(0);
            PlayerStatsManager.Instance.SetCurrentSouls(0);
            PlayerStatsManager.Instance.SetCurrentExp(0);

            UIManager.Instance.UpdateHealthPoints();
            UIManager.Instance.UpdateManaPoints();
            UIManager.Instance.UpdateSoulPoints();
            UIManager.Instance.UpdateExpBar();

            Progression.SetSpellPoints(0);
            Progression.UnlockedSpellsList.Clear();
            Progression.SetLastCheckpoint(new Vector3(103, -14, 0));
            IsCheckpointSaved = false;
            Progression.SetIsNewGame(false);
        }

        if (!IsTesting)
        {
            GameObject.FindWithTag("Player").transform.position = Progression.LastCheckpoint;

            if (GameObject.FindWithTag("CameraContainer") != null)
            {
                GameObject.FindWithTag("CameraContainer").transform.position = Progression.LastCheckpoint + new Vector3(12f, 8.4f, -10f);
            }
        }

        FindObjectOfType<PositionCameraToPlayer>().SetCameraToPlayer();
    }

    public void SetIsCheckpointSaved(bool value)
    {
        IsCheckpointSaved = value;
    }
}
