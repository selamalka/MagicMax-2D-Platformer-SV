using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    [SerializeField] private GameObject childObject;

    private void Update()
    {
        if (childObject != null) return;
        Destroy(gameObject);
    }
}
