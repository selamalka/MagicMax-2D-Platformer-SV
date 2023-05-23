using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Progression", menuName = "Progression")]
public class Progression : ScriptableObject
{
    [field: SerializeField] public List<SpellData> UnlockedSpellsList { get; private set; } = new List<SpellData>();
    [field: SerializeField] public Vector3 LastCheckpoint { get; private set; }

    public void SetLastCheckpoint(Vector3 lastCheckpoint)
    {
        LastCheckpoint = lastCheckpoint;
    }
}