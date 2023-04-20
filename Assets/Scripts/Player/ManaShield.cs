using UnityEngine;

public class ManaShield : MonoBehaviour
{
    [field: SerializeField] public SpellData SpellData { get; private set; }
    private float lifespanCounter;
    private Transform playerTransform;

    private void Start()
    {
        lifespanCounter = SpellData.Lifespan;
        playerTransform = GameObject.FindWithTag("Player").transform;
        transform.parent.SetParent(playerTransform);
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
            Destroy(gameObject);
        }
    }
}
