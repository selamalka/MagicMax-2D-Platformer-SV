using UnityEngine;
using UnityEngine.UI;

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
            if (CurrentSpell == null) return;
            ProgressionManager.Instance.Progression.SetSpellSlot1Info(CurrentSpell.SpellPrefab, CurrentSpell.SpellData);
        }
        else if (number == 2)
        {
            if (CurrentSpell == null) return;
            ProgressionManager.Instance.Progression.SetSpellSlot2Info(CurrentSpell.SpellPrefab, CurrentSpell.SpellData);
        }
    }

    public void LoadSpellSlotInfo(int number)
    {
        if (number == 1)
        {
            if (ProgressionManager.Instance.Progression.SpellSlot1Data == null) return;
            var spell = SpellManager.Instance.FindUISpellByName(ProgressionManager.Instance.Progression.SpellSlot1Data.Name);
            CurrentSpell = spell;
            CurrentSpell.SetSpellPrefab(ProgressionManager.Instance.Progression.SpellSlot1Prefab);
            CurrentSpell.SetSpellData(ProgressionManager.Instance.Progression.SpellSlot1Data);
            gameObject.GetComponent<Image>().sprite = CurrentSpell.GetComponent<Image>().sprite;
            gameObject.GetComponent<Image>().color = new Color(1,1,1,1);
        }
        else if (number == 2)
        {
            if (ProgressionManager.Instance.Progression.SpellSlot2Data == null) return;
            var spell = SpellManager.Instance.FindUISpellByName(ProgressionManager.Instance.Progression.SpellSlot2Data.Name);
            CurrentSpell = spell;
            CurrentSpell.SetSpellPrefab(ProgressionManager.Instance.Progression.SpellSlot2Prefab);
            CurrentSpell.SetSpellData(ProgressionManager.Instance.Progression.SpellSlot2Data);
            gameObject.GetComponent<Image>().sprite = CurrentSpell.GetComponent<Image>().sprite;
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
}