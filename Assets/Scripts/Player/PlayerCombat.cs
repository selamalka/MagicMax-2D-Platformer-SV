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
        if (PlayerStatsManager.Instance.CurrentMana == 0) return;
        var projectileContainer = Instantiate(magicShotContainer, projectileOriginSide.position, Quaternion.identity);
        projectileContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);
        var manaCost = projectileContainer.GetComponentInChildren<PlayerProjectile>().Data.ManaCost;
        PlayerStatsManager.Instance.UseMana(manaCost);
        EventManager.OnPlayerUseMana?.Invoke();
    }

    private void MeleeSlash()
    {
        var origin = GetProjectileOrigin();
        var meleeInstance = Instantiate(meleeSlashPrefab, GetProjectileOrigin(), Quaternion.identity, meleeInstancesParent);

        meleeInstance.transform.localScale = body.localScale;
        
        if (origin == projectileOriginTop.position)
        {
            meleeInstance.transform.rotation = PlayerController.Instance.IsFacingRight ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, -90);
        }
        else if (origin == projectileOriginBottom.position)
        {
            meleeInstance.transform.rotation = PlayerController.Instance.IsFacingRight ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
        }
    }

    private Vector3 GetProjectileOrigin()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                print("top");
                return projectileOriginTop.position;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                print("bottom");
                return projectileOriginBottom.position;
            }

            //print("side");
            return projectileOriginSide.position;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            //print("top");
            return projectileOriginTop.position;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //print("bottom");
            return projectileOriginBottom.position;
        }

        else return projectileOriginSide.position;
    }
}
