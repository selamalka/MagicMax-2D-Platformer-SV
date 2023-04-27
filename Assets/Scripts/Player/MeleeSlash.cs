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
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(0, 20);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(0, -20);
        }
        else
        {
            rb.velocity = new Vector2(PlayerController.Instance.IsFacingRight ? 20 : -20, 0);
        }

    }

    private void Update()
    {
        //UnstuckPlayer();
    }

    private async void FixedUpdate()
    {        
        if (shouldPush)
        {
            shouldPush = false;
            PlayerController.Instance.PushPlayerAgainstDirection(enemyDirection);
            await Task.Delay(100);
            PlayerController.Instance.SetIsControllable(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
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
        }
    }

    private async void UnstuckPlayer()
    {
        if (shouldPush == true && PlayerController.Instance.IsControllable == false)
        {
            await Task.Delay(50);
            PlayerController.Instance.SetIsControllable(true);
        }
    }
}
