using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    [field: SerializeField] public List<SpellData> Spells = new List<SpellData>();
    [field: SerializeField] public List<UISpell> UISpells = new List<UISpell>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerCombat.Instance.LoadSpellSlotsInfo();
    }

    public UISpell FindUISpellByName(string name)
    {
        UISpell spell = UISpells.FirstOrDefault(s => s.SpellName == name);
        return spell;
    }

    public SpellData FindSpellDataByNameAndLevel(string name, int level)
    {
        SpellData spell = Spells.FirstOrDefault(s => s.Name == name && s.Level == level);
        return spell;
    }
}