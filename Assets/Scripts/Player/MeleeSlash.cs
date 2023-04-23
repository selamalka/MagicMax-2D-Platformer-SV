using System.Threading.Tasks;
using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    [SerializeField] private int sidePushForce;
    [SerializeField] private int upPushForce;
    private Vector2 enemyDirection;
    private bool shouldPush;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            shouldPush = true;
            collision.GetComponent<IDamageable>().TakeDamage(PlayerStatsManager.Instance.MeleeDamage);
            GetComponent<Collider2D>().enabled = false;
            PlayerController.Instance.SetIsControllable(false);
            enemyDirection = collision.gameObject.transform.position - transform.position;
        }
    }

    private async void FixedUpdate()
    {
        if (shouldPush)
        {
            PlayerController.Instance.PushPlayerAgainstDirection(enemyDirection);
            shouldPush = false;
            await Task.Delay(100);
            PlayerController.Instance.SetIsControllable(true);
        }
    }
}
