using UnityEngine;

public class ImpAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private Animator animator;
    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private bool isTurning;

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private ImpStateType currentState;

    private GameObject playerGameObject;
    private float detectionRadius;

    private void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = EnemyManager.Instance.ImpSpeed;
        timeBetweenProjectiles = EnemyManager.Instance.ImpTimeBetweenProjectiles;
        projectilePrefab = EnemyManager.Instance.ImpProjetilePrefab;
        detectionRadius = EnemyManager.Instance.ImpDetectionRadius;
        projectileCooldownCounter = timeBetweenProjectiles;

        currentState = ImpStateType.Patrol;
        animator.SetBool("isWalking", true);
    }

    private void Update()
    {
        if (playerGameObject == null) return;

        switch (currentState)
        {
            case ImpStateType.Patrol:
                // Patrol() is in FixedUpdate
                if (IsPlayerInAttackRange())
                {
                    currentState = ImpStateType.Attack;
                    animator.SetBool("isWalking", false);
                }

                break;

            case ImpStateType.Attack:

                if (!IsPlayerInAttackRange())
                {
                    currentState = ImpStateType.Patrol;
                    animator.SetBool("isWalking", true);
                }

                AttackPlayer();
                TurnHandler();
                break;

            default:
                break;
        }

        ProjectileCooldownHandler();
    }

    private void FixedUpdate()
    {
        if (playerGameObject == null) return;

        if (currentState == ImpStateType.Patrol)
        {
            if (!isTurning)
            {
                Patrol();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PatrolPoint"))
        {
            if (currentState == ImpStateType.Patrol)
            {
                Turn();
            }
        }
    }

    private void AttackPlayer()
    {
        if (playerGameObject == null)
        {
            currentState = ImpStateType.Patrol;
            return;
        }

        if (projectileCooldownCounter <= 0)
        {
            //InstantiateProjectile();
            animator.SetTrigger("attack");
            projectileCooldownCounter = timeBetweenProjectiles;
        }
    }
    private void ProjectileCooldownHandler()
    {
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
        }
    }

    private bool IsPlayerInAttackRange()
    {
        return GetDistanceFromPlayer() <= detectionRadius;
    }

    private float GetDistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, playerGameObject.transform.position);
    }

    private void Turn()
    {
        isTurning = true;
        transform.rotation = Quaternion.Euler(0, isFacingRight ? 180f : 0, 0);
        isFacingRight = !isFacingRight;
        isTurning = false;
    }

    private void TurnHandler()
    {
        Vector3 direction = playerGameObject.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

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

    private void Patrol()
    {
        rb.velocity = new Vector2(transform.right.x * Time.deltaTime * speed, 0);
    }

    //Animation Event
    public void InstantiateProjectile()
    {
        Instantiate(projectilePrefab, projectilePointTransform.position, Quaternion.identity);
    }

    public void Knockback()
    {
        if (isFacingRight)
        {
            rb.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        }
    }
}
