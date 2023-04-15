using UnityEngine;

public class SpellSlot : MonoBehaviour
{
    [field: SerializeField] public SpellData CurrentSpell { get; private set; }

    public void SetCurrentSpell(SpellData spell)
    {
        CurrentSpell = spell;
    }

}
