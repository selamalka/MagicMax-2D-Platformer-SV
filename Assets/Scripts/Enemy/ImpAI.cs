using UnityEngine;

public class ImpAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private Vector3 startingPosition;
    [SerializeField] private Vector3 targetPosition;
    private Vector3 travelDistance = new Vector3(5, 0, 0);

    private void Start()
    {
        startingPosition = transform.position;
        targetPosition = startingPosition + travelDistance;

        rb = GetComponent<Rigidbody2D>();
        speed = EnemyStatsManager.Instance.ImpSpeed;
    }

    private void FixedUpdate()
    {
        TurnHandler();
        Move();
    }

    private void TurnHandler()
    {
        if (transform.position.x > targetPosition.x)
        {
            Turn();
        }
        else if (transform.position.x < startingPosition.x)
        {
            Turn();
        }
    }

    private void Move()
    {
        if (isFacingRight)
        {
            rb.velocity = new Vector2(transform.right.x * Time.deltaTime * speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-transform.right.x * Time.deltaTime * speed, 0);
        }
    }

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
}
