using UnityEngine;

public class EnemyProjectileContainer : MonoBehaviour
{
    [SerializeField] private GameObject childObject;

    private void Update()
    {
        if (childObject != null) return;
        Destroy(gameObject);
    }
}
