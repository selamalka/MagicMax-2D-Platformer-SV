using UnityEngine;

public class SpellSlot : MonoBehaviour
{
    [field: SerializeField] public int SpellSlotNumber { get; private set; }
    [field: SerializeField] public UISpell CurrentSpell { get; private set; }

    public void SetCurrentSpell(UISpell uiSpell)
    {
        CurrentSpell = uiSpell;
    }

    public void SaveSpellSlotInfo(int number)
    {
        if (number == 1)
        {
            ProgressionManager.Instance.Progression.SetSpellSlot1Info(CurrentSpell.SpellPrefab, CurrentSpell.SpellData);
        }
        else if (number == 2)
        {
            ProgressionManager.Instance.Progression.SetSpellSlot2Info(CurrentSpell.SpellPrefab, CurrentSpell.SpellData);
        }
    }

    public void LoadSpellSlotInfo(int number)
    {
        if (number == 1)
        {
            var spell = SpellManager.Instance.FindUISpellByName(ProgressionManager.Instance.Progression.SpellSlot1Data.Name);
            CurrentSpell = spell;
            CurrentSpell.SetSpellPrefab(ProgressionManager.Instance.Progression.SpellSlot1Prefab);
            CurrentSpell.SetSpellData(ProgressionManager.Instance.Progression.SpellSlot1Data);
        }
        else if (number == 2)
        {
            var spell = SpellManager.Instance.FindUISpellByName(ProgressionManager.Instance.Progression.SpellSlot2Data.Name);
            print(spell);
            CurrentSpell = spell;
            CurrentSpell.SetSpellPrefab(ProgressionManager.Instance.Progression.SpellSlot2Prefab);
            CurrentSpell.SetSpellData(ProgressionManager.Instance.Progression.SpellSlot2Data);
        }
    }
}