using System.Threading.Tasks;
using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    private Vector2 enemyDirection;
    private bool shouldPush;
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

    /*    private async void FixedUpdate()
        {        
            if (shouldPush)
            {
                shouldPush = false;
                PlayerController.Instance.PushPlayerAgainstDirection(enemyDirection);
                await Task.Delay(100);
                PlayerController.Instance.SetIsControllable(true);
            }
            else
            {
                UnstuckPlayer();
            }
        }*/

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            shouldPush = true;
            PlayerController.Instance.SetIsControllable(false);
            collision.GetComponent<IDamageable>().TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
            Transform enemyTransform = collision.gameObject.transform;
            PlayerCombat.Instance.InstantiateHitEffect(transform.position, enemyTransform);
            CameraShaker.Instance.Shake(5f, 0.1f);
            GetComponent<Collider2D>().enabled = false;
            enemyDirection = collision.gameObject.transform.position - transform.position;
            PlayerController.Instance.PushPlayerAgainstEnemyDirectionOnMelee(enemyDirection);
            await Task.Delay(150);
            PlayerController.Instance.SetIsControllable(true);
        }
    }

    private async void UnstuckPlayer()
    {
        if (shouldPush && PlayerController.Instance.IsControllable == false)
        {
            await Task.Delay(100);
            PlayerController.Instance.SetIsControllable(true);
        }
    }
}
