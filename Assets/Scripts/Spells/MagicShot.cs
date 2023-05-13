using UnityEngine;

public class MagicShot : MonoBehaviour
{
    [field: SerializeField] public SpellData SpellData { get; private set; }
    private float lifespanCounter;
    private int maxEnemiesHit;
    private int enemiesHitCounter;

    private void Start()
    {
        lifespanCounter = SpellData.Lifespan;

        switch (SpellData.Level)
        {
            case 1:
                maxEnemiesHit = 1;
                break;

            case 2:
                maxEnemiesHit = 2;
                break;

            case 3:
                maxEnemiesHit = 3;
                break;

            default:
                break;
        }
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
        transform.position += -transform.forward * SpellData.Speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (enemiesHitCounter < maxEnemiesHit)
            {
                collision.GetComponent<IDamageable>().TakeDamage(SpellData.Damage);
                enemiesHitCounter++;
                if (enemiesHitCounter == maxEnemiesHit)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

/*    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemiesHitCounter < maxEnemiesHit)
            {
                other.GetComponent<IDamageable>().TakeDamage(SpellData.Damage);
                enemiesHitCounter++;
                if (enemiesHitCounter == maxEnemiesHit)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (other.CompareTag("Tilemap"))
        {
            Destroy(gameObject);
        }
    }*/
}
