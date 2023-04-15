using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private SpellData data;
    private float lifespanCounter;
    public Vector3 Target { get; private set; }

    private void Start()
    {
        lifespanCounter = data.Lifespan;
    }

    private void Update()
    {
        LifespanHandler();
        Fly();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IDamageable>().TakeDamage(data.Damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Tilemap"))
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 target)
    {
        Target = target;
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
