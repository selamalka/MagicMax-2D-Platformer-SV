using UnityEngine;

public class FollowProjectile : MonoBehaviour
{
    [SerializeField] private Transform projectile;

    private void Update()
    {
        if (projectile == null) return;
        transform.position = projectile.position;
    }
}
