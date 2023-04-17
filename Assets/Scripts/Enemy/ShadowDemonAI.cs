using UnityEngine;

public class ShadowDemonAI : MonoBehaviour
{
    private ShadowDemonStateType currentState;
    [SerializeField] private Transform[] patrolPoints;
    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private int currentPatrolPointIndex;
    private Vector2 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = EnemyManager.Instance.ShadowDemonSpeed;
        velocity = new Vector2(speed, speed);

        currentState = ShadowDemonStateType.Patrol;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case ShadowDemonStateType.Patrol:
                Move();
                break;

            case ShadowDemonStateType.Attack:
                break;

            default:
                break;
        }
    }

    private void Move()
    {
        if (rb.position == (Vector2)patrolPoints[currentPatrolPointIndex].position)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        }

        Vector2 targetPosition = Vector2.MoveTowards(rb.position, patrolPoints[currentPatrolPointIndex].position, speed * Time.deltaTime);
        rb.MovePosition(targetPosition);
    }
}