using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform body;
    [SerializeField] private Transform projectileOriginSide;
    [SerializeField] private Transform projectileOriginTop;
    [SerializeField] private Transform projectileOriginBottom;
    [SerializeField] private Transform meleeInstancesParent;
    [SerializeField] private GameObject meleeSlashPrefab;
    [SerializeField] private SpellSlot spellSlot1;
    [SerializeField] private SpellSlot spellSlot2;

    private void OnEnable()
    {
        EventManager.OnWPressed += MeleeSlash;
        EventManager.OnQPressed += UseSpellSlot1;
        EventManager.OnEPressed += UseSpellSlot2;
    }

    private void OnDisable()
    {
        EventManager.OnWPressed -= MeleeSlash;
        EventManager.OnQPressed -= UseSpellSlot1;
        EventManager.OnEPressed -= UseSpellSlot2;
    }

    private void UseSpellSlot1()
    {
        if (spellSlot1.CurrentSpell == null) print("No spell is equipped");
        else if (PlayerStatsManager.Instance.CurrentMana == 0) print("Not enough mana");
        else
        {
            var spellContainer = Instantiate(spellSlot1.CurrentSpell.SpellPrefab, projectileOriginSide.position, Quaternion.identity);
            spellContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);

            if (PlayerStatsManager.Instance.IsEnoughMana(spellSlot1.CurrentSpell.SpellData.ManaCost))
            {
                PlayerStatsManager.Instance.UseMana(spellSlot1.CurrentSpell.SpellData.ManaCost);
            }
            else
            {
                Destroy(spellContainer);
                print("not enough mana");
            }
        }
    }
    private void UseSpellSlot2()
    {
        if (spellSlot2.CurrentSpell == null) print("No spell is equipped");
        else if (PlayerStatsManager.Instance.CurrentMana == 0) print("Not enough mana");
        else
        {
            var spellContainer = Instantiate(spellSlot2.CurrentSpell.SpellPrefab, projectileOriginSide.position, Quaternion.identity);
            spellContainer.transform.rotation = Quaternion.Euler(0, 0, PlayerController.Instance.IsFacingRight ? -90 : 90);

            if (PlayerStatsManager.Instance.IsEnoughMana(spellSlot2.CurrentSpell.SpellData.ManaCost))
            {
                PlayerStatsManager.Instance.UseMana(spellSlot2.CurrentSpell.SpellData.ManaCost);
            }
            else
            {
                Destroy(spellContainer);
                print("not enough mana");
            }
        }
    }

    private void MeleeSlash()
    {
        var origin = GetSpellOrigin();
        var meleeInstance = Instantiate(meleeSlashPrefab, GetSpellOrigin(), Quaternion.identity, meleeInstancesParent);

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

    private Vector3 GetSpellOrigin()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //print("top");
                return projectileOriginTop.position;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                //print("bottom");
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
