using System.Collections.Generic;
using UnityEngine;

public class SpellbookManager : MonoBehaviour
{
    public static SpellbookManager Instance;

    [field: SerializeField] public List<SpellData> UnlockedSpells = new List<SpellData>();

    private void Awake()
    {
        Instance = this;
    }
}
