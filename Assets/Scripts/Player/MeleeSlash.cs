using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 enemyDirection;
    private bool shouldPush;

    private void Start()
    {
       rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            shouldPush = true;
            collision.GetComponent<IDamageable>().TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
            enemyDirection = collision.gameObject.transform.position - transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (shouldPush)
        {
            PushPlayer(enemyDirection);
            shouldPush = false;
        }
    }

    private void PushPlayer(Vector2 enemyDirection)
    {
        if (enemyDirection.x > 0)
        {
            rb.velocity = Vector2.left * 50; 
            print(rb.velocity.x);
        }
        else if (enemyDirection.x < 0)
        {
            rb.velocity = Vector2.right * 50;
            print(rb.velocity.x);
        }
    }
}
