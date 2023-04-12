using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [field: SerializeField] public ProjectileData Data { get; private set; }
    private float lifespanCounter;

    private void Start()
    {
        lifespanCounter = Data.Lifespan;
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
        transform.position += transform.up * Data.Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IDamageable>().TakeDamage(Data.Damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Tilemap"))
        {
            Destroy(gameObject);
        }
    }
}
