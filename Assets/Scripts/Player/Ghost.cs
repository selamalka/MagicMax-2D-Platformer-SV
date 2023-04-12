using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Transform ghostInstancesParent;
    [SerializeField] private float ghostDelayStartTime;
    [SerializeField] private float ghostDestructionTime;
    
    private float ghostCounter;
    public bool ShouldCreateGhost { get; private set; }

    private void Start()
    {
        ghostCounter = ghostDelayStartTime;
    }

    private void Update()
    {
        GhostHandler();
    }

    private void GhostHandler()
    {
        ghostCounter -= Time.deltaTime;

        if (ghostCounter <= 0)
        {
            InstantiateGhost();
            ghostCounter = ghostDelayStartTime;
        }
    }

    private void InstantiateGhost()
    {
        var newPos = new Vector3(ghostInstancesParent.transform.position.x, ghostInstancesParent.transform.position.y, ghostInstancesParent.transform.position.z);
        var currentGhost = Instantiate(ghostPrefab, newPos, Quaternion.identity, ghostInstancesParent);
        Destroy(currentGhost, ghostDestructionTime);
    }
}
