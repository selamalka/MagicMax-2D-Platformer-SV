using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Data", menuName = "Projectile Data")]

public class ProjectileData : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int ManaCost { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float Lifespan { get; private set; }
}
