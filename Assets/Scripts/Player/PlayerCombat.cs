using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform projectileOriginSide;
    [SerializeField] private Transform projectileOriginTop;
    [SerializeField] private Transform projectileOriginBottom;
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
        var projectileContainer = Instantiate(magicShotContainer, projectileOriginSide.position, Quaternion.identity);
        projectileContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);

        EventManager.OnPlayerUseMana?.Invoke();
    }

    private void MeleeSlash()
    {
        var meleeInstance = Instantiate(meleeSlashPrefab, GetProjectileOrigin(), Quaternion.identity, meleeInstancesParent);
        meleeInstance.transform.localScale = body.localScale;
    }

    private Vector3 GetProjectileOrigin()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            return projectileOriginSide.position;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            return projectileOriginTop.position;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            return projectileOriginBottom.position;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            return projectileOriginTop.position;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            return projectileOriginBottom.position;
        }
        else return projectileOriginSide.position;
    }
}
