using System.Threading.Tasks;
using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    private Vector2 enemyDirection;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.localScale = new Vector3(PlayerController.Instance.IsFacingRight ? 1 : -1, 1, 1);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(0, 20);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(0, -50);
        }
        else
        {
            rb.velocity = new Vector2(PlayerController.Instance.IsFacingRight ? 20 : -20, 0);
        }
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            PlayerController.Instance.SetIsControllable(false);

            collision.GetComponent<IDamageable>().TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
            Transform enemyTransform = collision.gameObject.transform;
            PlayerCombat.Instance.InstantiateHitEffect(transform.position, enemyTransform);
            GetComponent<Collider2D>().enabled = false;

            enemyDirection = collision.gameObject.transform.position - transform.position;
            PlayerController.Instance.PushPlayerAgainstEnemyDirectionOnMelee(enemyDirection);

            if (collision.gameObject.GetComponent<ImpAI>() != null)
            {
                collision.gameObject.GetComponent<ImpAI>().Knockback(); 
            }

            await Task.Delay(150);
            PlayerController.Instance.SetIsControllable(true);
        }
    }
}
