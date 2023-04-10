using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private ProjectileData data;
    private float lifespanCounter;

    private void Start()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IDamageable>().TakeDamage(data.Damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Tilemap"))
        {
            Destroy(gameObject);
        }
    }
}
