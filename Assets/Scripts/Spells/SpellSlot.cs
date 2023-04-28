using UnityEngine;

public class SpellSlot : MonoBehaviour
{
    [field: SerializeField] public UISpell CurrentSpell { get; private set; }

    public void SetCurrentSpell(UISpell uiSpell)
    {
        CurrentSpell = uiSpell;
    }
}