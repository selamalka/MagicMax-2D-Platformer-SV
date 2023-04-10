using UnityEngine;

public class ProjectileContainer : MonoBehaviour
{
    [SerializeField] private GameObject childProjectile;    

    private void Update()
    {
        if (childProjectile == null)
        { 
            Destroy(gameObject); 
        }
    }
}
