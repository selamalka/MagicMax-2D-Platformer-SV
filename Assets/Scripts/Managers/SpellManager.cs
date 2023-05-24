using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public static SpellManager Instance;

    [field: SerializeField] public List<SpellData> Spells = new List<SpellData>();
    [field: SerializeField] public List<GameObject> UISpellsPrefabs = new List<GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    public GameObject FindUISpellPrefabByName(string name)
    {
        GameObject spell = UISpellsPrefabs.FirstOrDefault(s => s.name == name);
        return spell;
    }

    public SpellData FindSpellDataByNameAndLevel(string name, int level)
    {
        SpellData spell = Spells.FirstOrDefault(s => s.Name == name && s.Level == level);
        return spell;
    }
}