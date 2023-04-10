using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private Transform meleeInstancesParent;
    [SerializeField] private Transform body;
    [SerializeField] private GameObject magicShotPrefab;
    [SerializeField] private GameObject meleeSlashPrefab;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EventManager.OnEPressed += MeleeSlash;
        EventManager.OnQPressed += LaunchProjectile;
    }

    private void OnDisable()
    {
        EventManager.OnEPressed -= MeleeSlash;
        EventManager.OnQPressed -= LaunchProjectile;
    }

    private void LaunchProjectile()
    {
        var projectileInstance = Instantiate(magicShotPrefab, projectileOrigin.position, Quaternion.Euler(0,0,-90));
        var modifiedLocalScale = projectileInstance.transform.localScale;
        modifiedLocalScale.x *= Mathf.Sign(projectileInstance.transform.localScale.x);
        projectileInstance.transform.localScale = modifiedLocalScale;
    }

    private void MeleeSlash()
    {
        var meleeInstance = Instantiate(meleeSlashPrefab, projectileOrigin.position, Quaternion.identity, meleeInstancesParent);
        meleeInstance.transform.localScale = body.localScale;
    }
}
