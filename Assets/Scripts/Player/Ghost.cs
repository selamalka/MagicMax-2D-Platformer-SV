using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private Transform ghostOriginTransform;
    [SerializeField] private Transform body;
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
        if (!ShouldCreateGhost) return;

        ghostCounter -= Time.deltaTime;

        if (ghostCounter <= 0)
        {
            InstantiateGhost();
            ghostCounter = ghostDelayStartTime;
        }
    }

    private void InstantiateGhost()
    {
        var newPos = new Vector3(ghostOriginTransform.transform.position.x, ghostOriginTransform.transform.position.y, ghostOriginTransform.transform.position.z);
        var currentGhost = Instantiate(ghostPrefab, newPos, Quaternion.identity);
        currentGhost.transform.localScale = body.localScale;
        Destroy(currentGhost, ghostDestructionTime);
    }

    public void SetShouldCreateGhost(bool value)
    {
        ShouldCreateGhost = value;
    }
}