using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Progression", menuName = "Progression")]
public class Progression : ScriptableObject
{
    [field: SerializeField] public bool IsNewGame {  get; private set; }
    [field: SerializeField] public List<SpellData> UnlockedSpellsList { get; private set; } = new List<SpellData>();
    [field: SerializeField] public Vector3 LastCheckpoint { get; private set; }
    [field: SerializeField] public int SpellPoints { get; private set; }

    [field: SerializeField] public GameObject SpellSlot1 { get; private set; }
    [field: SerializeField] public GameObject SpellSlot2 { get; private set; }

    public void SetLastCheckpoint(Vector3 lastCheckpoint)
    {
        LastCheckpoint = lastCheckpoint;
    }

    public void SetSpellPoints(int spellPoints)
    {
        SpellPoints = spellPoints;
    }

    public void SetIsNewGame(bool value)
    {
        IsNewGame = value;
    }
}