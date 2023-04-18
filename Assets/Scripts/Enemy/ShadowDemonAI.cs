using UnityEngine;

public class ShadowDemonAI : MonoBehaviour
{
    private ShadowDemonStateType currentState;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform body;
    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private int currentPatrolPointIndex;
    private GameObject projectilePrefab;    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = EnemyManager.Instance.ShadowDemonSpeed;
        projectilePrefab = EnemyManager.Instance.ShadowDemonProjetilePrefab;
        transform.position = patrolPoints[0].position;

        currentState = ShadowDemonStateType.Patrol;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case ShadowDemonStateType.Patrol:
                Move();
                TurnHandler();
                break;

            case ShadowDemonStateType.Attack:
                Attack();
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = ShadowDemonStateType.Attack;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = ShadowDemonStateType.Patrol;
        }
    }

    private void Move()
    {
/*        if (rb.position == (Vector2)patrolPoints[currentPatrolPointIndex].position)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        }
        Vector2 targetPosition = Vector2.MoveTowards(rb.position, patrolPoints[currentPatrolPointIndex].position, speed * Time.deltaTime);
        rb.MovePosition(targetPosition);*/

        if (Vector2.Distance(rb.position, (Vector2)patrolPoints[currentPatrolPointIndex].position) < 0.1f)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        }

        Vector2 direction = (patrolPoints[currentPatrolPointIndex].position - transform.position).normalized;
        rb.velocity = speed * Time.deltaTime * direction;
    }

    private void TurnHandler()
    {
        if (isFacingRight && rb.velocity.x < 0)
        {
            Turn();
        }
        else if (!isFacingRight && rb.velocity.x > 0)
        {
            Turn();
        }
    }

    private void Turn()
    {
        rb.velocity = Vector2.zero;
        Vector3 scale = body.transform.localScale;
        scale.x *= -1;
        body.transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    private void Attack()
    {
        
    }
}