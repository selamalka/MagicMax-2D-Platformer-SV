using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb;
    private Vector2 enemyDirection;
    private bool shouldPush;

    private void Start()
    {
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
        if (Input.GetKey(KeyCode.UpArrow)) return;

        if (enemyDirection.x > 0)
        {
            rb.velocity = Vector2.left * 50;
        }
        else if (enemyDirection.x < 0)
        {
            rb.velocity = Vector2.right * 50;
        }

        if (!playerController.IsGrounded && Input.GetKey(KeyCode.DownArrow) && enemyDirection.y < 0)
        {
            rb.velocity = Vector2.up * 25;
        }
    }
}
