using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private ProjectileData data;
    private float lifespanCounter;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = data.Sprite;
        lifespanCounter = data.Lifespan;
    }

    private void Update()
    {
        LifespanHandler();
        Fly();
    }

    private void LifespanHandler()
    {
        lifespanCounter -= Time.deltaTime;

        if (lifespanCounter <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Fly()
    {        
        transform.position += transform.up * data.Speed * Time.deltaTime;        
    }
}
