using UnityEngine;

public class ManaShield : MonoBehaviour
{
    [field: SerializeField] public SpellData SpellData { get; private set; }
    private float lifespanCounter;
    private Transform playerTransform;
    private int maxProjectilesDefelected;
    private int deflectedProjectilesCounter;

    private void Awake()
    {
        if (FindObjectsOfType<ManaShield>().Length >= 2)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerStatsManager.Instance.CurrentMana == 0) Destroy(gameObject);
        lifespanCounter = SpellData.Lifespan;
        playerTransform = GameObject.FindWithTag("Player").transform;
        transform.parent.SetParent(playerTransform);

        switch (SpellData.Level)
        {
            case 1:
                maxProjectilesDefelected = 1;
                break;

            case 2:
                maxProjectilesDefelected = 2;
                break;

            case 3:
                maxProjectilesDefelected = 3;
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        transform.parent.position = playerTransform.position + new Vector3(0, 0.5f, 0);
        LifespanHandler();
    }

    private void LifespanHandler()
    {
        lifespanCounter -= Time.deltaTime;

        if (lifespanCounter <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyProjectile>() != null)
        {
            Destroy(collision.GetComponent<EnemyProjectile>().gameObject);
            PlayerStatsManager.Instance.UseMana(1);
            deflectedProjectilesCounter++;
            if (deflectedProjectilesCounter == maxProjectilesDefelected)
            {
                Destroy(gameObject);
            }
        }
    }
}
