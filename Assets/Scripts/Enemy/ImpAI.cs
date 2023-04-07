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

        if (!isAttacking)
        {
            TurnHandler(groundCheck);
            Move();
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
            if (isAttacking == false)
            {
                isAttacking = true;
            }

            playerPosition = collision.transform.position;
            InstantiateProjectile(playerPosition);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAttacking = false;
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
