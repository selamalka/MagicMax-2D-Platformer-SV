using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlot : MonoBehaviour, IDropHandler
{
    [field: SerializeField] public SpellData CurrentSpell { get; private set; }

    public void SetCurrentSpell(SpellData spell)
    {
        CurrentSpell = spell;
    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
