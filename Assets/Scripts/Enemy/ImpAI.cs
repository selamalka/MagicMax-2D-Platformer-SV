using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ImpAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform groundCheckTransform;

    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private Vector3 playerPosition;
    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private bool isAttacking;
    private float waitTime = 2f;
    private bool isWaiting;
    private ImpStateType currentState;

    [SerializeField] private Vector3 startingPosition;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckTransform.position, new Vector3(0.5f, 0.5f, 0f));
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
        var groundCheck = Physics2D.OverlapBox(groundCheckTransform.position, new Vector2(0.5f, 0.5f), 0f);

        switch (currentState)
        {
            case ImpStateType.Patrol:
                TurnHandler(groundCheck);
                Move();
                break;

            case ImpStateType.Attack:
                FacePlayer();
                break;

            default:
                break;
        }
    }

    private void TurnHandler(Collider2D groundCheck)
    {
        if (!groundCheck.CompareTag("Tilemap"))
        {
            Turn();
        }
        else if (Mathf.Abs(transform.position.x - startingPosition.x) > travelDistance)
        {
            Turn();
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition = collision.transform.position;
            InstantiateProjectile(playerPosition);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = ImpStateType.Patrol;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = ImpStateType.Attack;

            playerPosition = collision.transform.position;

            var direction = playerPosition - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle > 90f || angle < -90f)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -180, transform.localEulerAngles.z);
            }
            else
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            } 
        }
    }

    private void FacePlayer()
    {
        var direction = playerPosition - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        print(angle);

        if (angle > 90f || angle < -90f)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
        }
        else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -180, transform.localEulerAngles.z);
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
