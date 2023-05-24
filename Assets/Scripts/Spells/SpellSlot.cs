using UnityEngine;

public class SpellSlot : MonoBehaviour
{
    [field: SerializeField] public int SpellSlotNumber { get; private set; }
    [field: SerializeField] public UISpell CurrentSpell { get; private set; }

    private void Start()
    {
        PlayerCombat.Instance.LoadSpellSlotsInfo();
    }

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
            GameObject CurrentSpellGameObject = SpellManager.Instance.FindUISpellPrefabByName(ProgressionManager.Instance.Progression.SpellSlot1Data.name);
            CurrentSpell = CurrentSpellGameObject.GetComponent<UISpell>();
            print(gameObject.name + " " + CurrentSpell);
            CurrentSpell.SetSpellPrefab(ProgressionManager.Instance.Progression.SpellSlot1Prefab);
            CurrentSpell.SetSpellData(ProgressionManager.Instance.Progression.SpellSlot1Data);
        }
        else if (number == 2)
        {
            GameObject CurrentSpellGameObject = SpellManager.Instance.FindUISpellPrefabByName(ProgressionManager.Instance.Progression.SpellSlot2Data.name);
            CurrentSpell = CurrentSpellGameObject.GetComponent<UISpell>();
            print(gameObject.name + " " + CurrentSpell);
            CurrentSpell.SetSpellPrefab(ProgressionManager.Instance.Progression.SpellSlot2Prefab);
            CurrentSpell.SetSpellData(ProgressionManager.Instance.Progression.SpellSlot2Data);
        }
    }
}