using UnityEngine;

public class PowerupBase : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivatePowerup();
        }
    }

    public virtual void ActivatePowerup() 
    {

    }
}
