using UnityEngine;

public class ImpAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private bool isTurning;

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private ImpStateType currentState;

    private Vector3 playerPosition;
    private Vector3 startingPosition;
    private float travelDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;

        speed = EnemyManager.Instance.ImpSpeed;
        timeBetweenProjectiles = EnemyManager.Instance.ImpTimeBetweenShots;
        projectilePrefab = EnemyManager.Instance.ImpProjetilePrefab;
        travelDistance = EnemyManager.Instance.ImpTravelDistance;
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
                Move();
                break;

            case ImpStateType.Attack:
                TurnHandler();
                InstantiateProjectile(playerPosition);
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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
        if (collision.CompareTag("PatrolPoint"))
        {
            if (currentState == ImpStateType.Patrol)
            {
                Turn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = ImpStateType.Patrol;
        }
    }

    private void Turn()
    {
        rb.velocity = Vector3.zero;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    private void TurnHandler()
    {
        //playerPosition = collision.transform.position;
        playerPosition = GameObject.FindWithTag("Player").transform.position;
        var direction = playerPosition - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

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

    private void InstantiateProjectile(Vector3 targetPosition)
    {
        if (projectilePrefab == null) return; 
        if (projectileCooldownCounter <= 0)
        {
            var direction = targetPosition - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var projectile = Instantiate(projectilePrefab, projectilePointTransform.position, Quaternion.Euler(new Vector3(0, 0, angle - 90)));
            projectile.GetComponent<EnemyProjectile>().SetTarget(targetPosition);
            projectileCooldownCounter = timeBetweenProjectiles;
        }
    }

}
