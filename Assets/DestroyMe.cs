using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    [SerializeField] private float lifespan;

    private void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }
}
