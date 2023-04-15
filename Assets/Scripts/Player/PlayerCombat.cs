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
        EventManager.OnWPressed += MeleeSlash;
        EventManager.OnQPressed += UseSpellSlot1;
    }

    private void OnDisable()
    {
        EventManager.OnWPressed -= MeleeSlash;
        EventManager.OnQPressed -= UseSpellSlot1;
    }

    private void UseSpellSlot1()
    {
        if (PlayerStatsManager.Instance.CurrentMana == 0) return;

        var spellContainer = Instantiate(magicShotContainer, projectileOriginSide.position, Quaternion.identity);
        spellContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);
        var manaCost = spellContainer.GetComponentInChildren<MagicShot>().SpellData.ManaCost;

        if (PlayerStatsManager.Instance.IsEnoughMana(manaCost))
        {
            PlayerStatsManager.Instance.UseMana(manaCost); 
        }
        else
        {
            Destroy(spellContainer);
            print("not enough mana");
        }
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
