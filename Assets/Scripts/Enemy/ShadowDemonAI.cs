using UnityEngine;

public class ShadowDemonAI : MonoBehaviour
{
    private ShadowDemonStateType currentState;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform body;
    [SerializeField] private Transform projectilePointTransform;
    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private int currentPatrolPointIndex;
    private GameObject projectilePrefab;

    private GameObject playerGameObject;
    private float detectionRadius;
    [SerializeField] private LayerMask playerLayer;
    private Vector3 playerPosition;
    private float maxDistanceFromPlayer;
    private bool isTurning;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;

    private void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        speed = EnemyManager.Instance.ShadowDemonSpeed;
        projectilePrefab = EnemyManager.Instance.ShadowDemonProjetilePrefab;
        maxDistanceFromPlayer = EnemyManager.Instance.ShadowDemonMaxDistanceFromPlayer;
        timeBetweenProjectiles = EnemyManager.Instance.ShadowDemonTimeBetweenProjectiles;
        detectionRadius = EnemyManager.Instance.ShadowDemonDetectionRadius;

        transform.position = patrolPoints[0].position;
        projectileCooldownCounter = timeBetweenProjectiles;

        currentState = ShadowDemonStateType.Patrol;
    }

    private void Update()
    {
        if (playerGameObject == null) return;

        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
        }

        Vector2 direction = playerGameObject.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            currentState = ShadowDemonStateType.Attack;
        }

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


    private void Patrol()
    {
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) < 0.1f)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPointIndex].position, speed * Time.deltaTime);
        TurnHandler(patrolPoints[currentPatrolPointIndex].position);
    }

    private void ChasePlayerAndAttack()
    {
        if (playerGameObject == null)
        {
            currentState = ShadowDemonStateType.Patrol;
            return;
        }

        playerPosition = playerGameObject.transform.position;
        if (Vector2.Distance(transform.position, playerPosition) > maxDistanceFromPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime); 
        }

        var direction = playerPosition - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        FaceTowardsPlayer(angle);

        AttackPlayerInRange();
    }

    private void AttackPlayerInRange()
    {
        if (IsPlayerInAttackRange())
        {
            if (projectileCooldownCounter <= 0)
            {
                InstantiateProjectile();
                projectileCooldownCounter = timeBetweenProjectiles;
            }
        }
    }

    private bool IsPlayerInAttackRange()
    {
        return Vector2.Distance(transform.position, playerPosition) <= detectionRadius;
    }

    private void InstantiateProjectile()
    {
        Instantiate(projectilePrefab, projectilePointTransform.position, Quaternion.identity);
    }

    private void TurnHandler(Vector3 nextPatrolPoint)
    {
        Vector3 direction = nextPatrolPoint - transform.position;

        if (isFacingRight && direction.x < 0)
        {
            Turn();
        }
        else if (!isFacingRight && direction.x > 0)
        {
            Turn();
        }
    }

    private void Turn()
    {
        Vector3 scale = body.transform.localScale;
        scale.x *= -1;
        body.transform.localScale = scale;
        isFacingRight = !isFacingRight;
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