using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    [field: SerializeField] public Progression Progression { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject.Find("Player").transform.position = Progression.LastCheckpoint;
        FindObjectOfType<PositonCameraToPlayer>().SetCameraToPlayer();
    }
}
