using UnityEngine;

public class FlameBurst : MonoBehaviour
{
    [field: SerializeField] public SpellData SpellData { get; private set; }
    private float lifespanCounter;
    private GameObject playerGameObject;
    private int maxEnemiesHit;
    private int enemiesHitCounter;

    private void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        transform.parent.SetParent(playerGameObject.transform);
        transform.parent.position = playerGameObject.GetComponent<Collider2D>().bounds.center;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(SpellData.Damage);
            enemiesHitCounter++;
            if (enemiesHitCounter == maxEnemiesHit)
            {
                Destroy(gameObject);
            }
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
}
