using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private Transform meleeInstancesParent;
    [SerializeField] private Transform body;
    [SerializeField] private GameObject magicShotContainer;
    [SerializeField] private GameObject meleeSlashPrefab;

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
        var projectileContainer = Instantiate(magicShotContainer, projectileOrigin.position, Quaternion.identity);
        projectileContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);
    }

    private void MeleeSlash()
    {
        var meleeInstance = Instantiate(meleeSlashPrefab, projectileOrigin.position, Quaternion.identity, meleeInstancesParent);
        meleeInstance.transform.localScale = body.localScale;
    }
}
