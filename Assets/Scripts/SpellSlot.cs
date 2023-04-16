using UnityEngine;

public class SpellSlot : MonoBehaviour
{
    [field: SerializeField] public Spell CurrentSpell { get; private set; }

    public void SetCurrentSpell(Spell spell)
    {
        CurrentSpell = spell;
    }
}
