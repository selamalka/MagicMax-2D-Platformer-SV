using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private SpellData data;
    private float lifespanCounter;
    private GameObject player;

    private void Start()
    {
        lifespanCounter = data.Lifespan;

        player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Vector3 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
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
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
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
