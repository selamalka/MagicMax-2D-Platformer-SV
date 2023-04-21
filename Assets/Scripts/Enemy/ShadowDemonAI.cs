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
    private float maxDistanceFromPlayer;
    private bool isTurning;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private Vector3 directionToPlayer;

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

        switch (currentState)
        {
            case ShadowDemonStateType.Patrol:

                if (IsPlayerInAttackRange())
                {
                    currentState = ShadowDemonStateType.Attack;
                }

                Patrol();
                break;

            case ShadowDemonStateType.Attack:

                ProjectileCooldownHandler();
                ChasePlayerAndAttack();
                break;

            default:
                break;
        }
    }

    private void ProjectileCooldownHandler()
    {
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
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

        if (GetDistanceFromPlayer() > maxDistanceFromPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerGameObject.transform.position, speed * Time.deltaTime);
        }

        var direction = playerGameObject.transform.position - transform.position;
        directionToPlayer = direction;
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

    private float GetDistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, playerGameObject.transform.position);
    }

    private bool IsPlayerInAttackRange()
    {
        return GetDistanceFromPlayer() <= detectionRadius;
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
        isTurning = true;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
        isTurning = false;
    }

    private void FaceTowardsPlayer(float angle)
    {
        if (angle > 90f || angle < -90f)
        {
            if (isFacingRight && !isTurning)
            {
                Turn();
            }
        }
        else if (angle < 90 || angle > -90)
        {
            if (!isFacingRight && !isTurning)
            {
                Turn();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {        
        if (collision.CompareTag("Tilemap"))
        {
            print(collision.gameObject);
            transform.position = Vector2.MoveTowards(transform.position, playerGameObject.transform.position, speed * 1.2f * Time.deltaTime);
        }
    }
}