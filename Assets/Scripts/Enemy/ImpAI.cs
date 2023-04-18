using UnityEngine;

public class ImpAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform body;

    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private bool isTurning;

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private ImpStateType currentState;

    private GameObject playerGameObject;
    private Vector3 playerPosition;
    private float attackRange;

    private void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        speed = EnemyManager.Instance.ImpSpeed;
        timeBetweenProjectiles = EnemyManager.Instance.ImpTimeBetweenProjectiles;
        projectilePrefab = EnemyManager.Instance.ImpProjetilePrefab;
        attackRange = EnemyManager.Instance.ImpAttackRange;
        projectileCooldownCounter = timeBetweenProjectiles;

        currentState = ImpStateType.Patrol;
    }

    private void Update()
    {
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case ImpStateType.Patrol:
                Patrol();
                break;

            case ImpStateType.Attack:
                AttackPlayer();
                break;

            default:
                break;
        }
    }

    private void AttackPlayer()
    {
        if (playerGameObject == null)
        {
            currentState = ImpStateType.Patrol;
            return;
        }
        playerPosition = playerGameObject.transform.position;

        Vector3 direction = playerPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        print(angle);
        TurnHandler(angle);
        AttackPlayerInRange(angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            currentState = ImpStateType.Attack;
            projectileCooldownCounter = timeBetweenProjectiles;
        }
        else if (collision.CompareTag("PatrolPoint"))
        {
            if (currentState == ImpStateType.Patrol)
            {
                Turn();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            currentState = ImpStateType.Patrol;
        }
    }

    private void Turn()
    {
        rb.velocity = Vector3.zero;
        Vector3 scale = body.transform.localScale;
        scale.x *= -1;
        body.transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    private void TurnHandler(float angle)
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

    private void Patrol()
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

    private void AttackPlayerInRange(float angle)
    {
        if (IsPlayerInAttackRange())
        {
            if (projectileCooldownCounter <= 0)
            {
                InstantiateProjectile(angle);
                projectileCooldownCounter = timeBetweenProjectiles;
            }
        }
    }

    private bool IsPlayerInAttackRange()
    {
        return Vector2.Distance(transform.position, playerPosition) <= attackRange;
    }

    private void InstantiateProjectile(float angle)
    {
        var projectile = Instantiate(projectilePrefab, projectilePointTransform.position, Quaternion.Euler(new Vector3(0, 0, angle -90)));
    }
}
