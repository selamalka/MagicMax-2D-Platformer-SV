using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

[CreateAssetMenu(fileName = "New Progression", menuName = "Progression")]
public class Progression : ScriptableObject
{
    [field: SerializeField] public bool IsNewGame {  get; private set; }
    [field: SerializeField] public List<SpellData> UnlockedSpellsList { get; private set; } = new List<SpellData>();
    [field: SerializeField] public Vector3 LastCheckpoint { get; private set; }
    [field: SerializeField] public float ExpPoints { get; private set; }
    [field: SerializeField] public float TargetExp { get; private set; }
    [field: SerializeField] public int SpellPoints { get; private set; }
    [field: SerializeField] public int HealthPoints { get; private set; }
    [field: SerializeField] public int ManaPoints { get; private set; }
    [field: SerializeField] public int SoulPoints { get; private set; }

    [field: SerializeField] public GameObject SpellSlot1Prefab { get; private set; }
    [field: SerializeField] public SpellData SpellSlot1Data { get; private set; }
    [field: SerializeField] public GameObject SpellSlot2Prefab { get; private set; }
    [field: SerializeField] public SpellData SpellSlot2Data { get; private set; }
    
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
    public void SetHealthPoints(int value)
    {
        HealthPoints = value;
    }
    public void SetManaPoints(int value)
    {
        ManaPoints = value;
    }
    public void SetSoulPoints(int value)
    {
        SoulPoints = value;
    }
    public void SetCurrentExpPoints(float value)
    {
        ExpPoints = value;
    }
    public void SetTargetExp(float value)
    {
        TargetExp = value;
    }

    public void SetSpellSlot1Info(GameObject prefab, SpellData data)
    {
        SpellSlot1Prefab = prefab;
        SpellSlot1Data = data;
    }
    public void SetSpellSlot2Info(GameObject prefab, SpellData data)
    {
        SpellSlot2Prefab = prefab;
        SpellSlot2Data = data;
    }
}