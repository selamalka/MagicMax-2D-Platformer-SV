using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    [field: SerializeField] public List<SpellData> Spells = new List<SpellData>();

    private void Awake()
    {
        Instance = this;
    }

    public SpellData FindSpellByNameAndLevel(string name, int level)
    {
        SpellData spell = Spells.FirstOrDefault(s => s.Name == name && s.Level == level);
        return spell;
    }
}
