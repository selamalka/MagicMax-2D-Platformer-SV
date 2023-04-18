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

    private GameObject playerGameObject;
    private Vector3 playerPosition;
    private float maxDistanceFromPlayer;
    private bool isTurning;

    private void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        speed = EnemyManager.Instance.ShadowDemonSpeed;
        projectilePrefab = EnemyManager.Instance.ShadowDemonProjetilePrefab;
        maxDistanceFromPlayer = EnemyManager.Instance.ShadowDemonMaxDistanceFromPlayer;
        transform.position = patrolPoints[0].position;

        currentState = ShadowDemonStateType.Patrol;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case ShadowDemonStateType.Patrol:
                Patrol();
                break;

            case ShadowDemonStateType.Attack:
                ChasePlayerAndAttack();
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

    private void Patrol()
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

        TurnHandler();
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

    private void ChasePlayerAndAttack()
    {
        playerPosition = playerGameObject.transform.position;

        var direction = playerPosition - transform.position;

        if (Vector2.Distance(rb.position, playerPosition) <= maxDistanceFromPlayer)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = speed * Time.deltaTime * direction.normalized;
        }

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        FaceTowardsPlayer(angle);
    }

    private void FaceTowardsPlayer(float angle)
    {
        if (angle > 90f || angle < -90f)
        {
            if (isFacingRight && !isTurning)
            {
                isTurning = true;
                Turn();
                isTurning = false;
            }
        }
        else if (angle < 90 || angle > -90)
        {
            if (!isFacingRight && !isTurning)
            {
                isTurning = true;
                Turn();
                isTurning = false;
            }
        }
    }
}