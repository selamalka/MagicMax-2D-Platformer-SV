using System.Threading.Tasks;
using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    private Vector2 enemyDirection;
    private bool shouldPush;

    private void Update()
    {
        //UnstuckPlayer();
    }

    private async void UnstuckPlayer()
    {
        if (shouldPush == false && PlayerController.Instance.IsControllable == false)
        {
            await Task.Delay(50);
            PlayerController.Instance.SetIsControllable(true);
        }
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
            GetComponent<Collider2D>().enabled = false;
            enemyDirection = collision.gameObject.transform.position - transform.position;
        }
    }
}
