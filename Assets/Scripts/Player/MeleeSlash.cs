using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            PlayerController.Instance.SetIsControllable(false);
            shouldPush = true;
            collision.GetComponent<IDamageable>().TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
            GetComponent<Collider2D>().enabled = false;
            enemyDirection = collision.gameObject.transform.position - transform.position;
        }
    }

    private async void FixedUpdate()
    {
        if (shouldPush)
        {
            PushPlayer(enemyDirection);
            shouldPush = false;
            await Task.Delay(100);
            PlayerController.Instance.SetIsControllable(true);
        }
    }

    private void PushPlayer(Vector2 enemyDirection)
    {
        if (Input.GetKey(KeyCode.UpArrow)) return;

        if (enemyDirection.x > 0)
        {
            rb.velocity = Vector2.left * 15;
            //rb.AddForce(Vector2.left * 15, ForceMode2D.Impulse);
        }
        else if (enemyDirection.x < 0)
        {
            rb.velocity = Vector2.right * 15;
            //rb.AddForce(Vector2.right * 15, ForceMode2D.Impulse);
        }

        if (!PlayerController.Instance.IsGrounded && Input.GetKey(KeyCode.DownArrow) && enemyDirection.y < 0)
        {
            rb.velocity = Vector2.up * 25;
        }
    }
}
