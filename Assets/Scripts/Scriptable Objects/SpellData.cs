using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Data", menuName = "Spell Data")]

public class SpellData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int ManaCost { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Lifespan { get; private set; }
}
