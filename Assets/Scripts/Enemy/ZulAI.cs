using System;
using UnityEngine;

public class ZulAI : MonoBehaviour
{
    [SerializeField] private Transform bodyContainerTransform;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private GameObject projectilePrefab;
    private int currentPatrolPointIndex;
    private ZulStateType currentState;
    private Vector3 playerPosition;
    private bool isTurning;
    private bool isFacingRight;
    private float speed;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private float flyStateStartTime;
    private float flyStateCounter;
    [SerializeField] private float AttackStateStartTime;
    private float AttackStateCounter;

    private float startTimeBetweenProjectiles;
    private float projectileCooldownCounter;

    private void Start()
    {
        playerPosition = PlayerController.Instance.transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        flyStateCounter = flyStateStartTime;
        currentState = ZulStateType.Fly;
        speed = EnemyManager.Instance.ZulSpeed;
        startTimeBetweenProjectiles = EnemyManager.Instance.ZulTimeBetweenProjectiles;
        projectileCooldownCounter = startTimeBetweenProjectiles;

    }

    private void Update()
    {
        playerPosition = PlayerController.Instance.transform.position;

        TurnHandler();

        switch (currentState)
        {
            case ZulStateType.Fly:

                if (flyStateCounter <= 0)
                {
                    AttackStateCounter = AttackStateStartTime;
                    projectileCooldownCounter = startTimeBetweenProjectiles;
                    currentState = ZulStateType.Attack;
                }
                else
                {
                    flyStateCounter -= Time.deltaTime;
                    Fly();
                }

                break;

            case ZulStateType.Attack:
                if (AttackStateCounter <= 0)
                {
                    flyStateCounter = flyStateStartTime;
                    currentState = ZulStateType.Fly;
                }
                else
                {
                    AttackStateCounter -= Time.deltaTime;
                    ProjectileHandler();
                }
                break;

            case ZulStateType.Summon:
                break;

            default:
                break;
        }
    }

    private void ProjectileHandler()
    {
        if (projectileCooldownCounter <= 0)
        {
            Attack();
            projectileCooldownCounter = startTimeBetweenProjectiles;
        }
        else
        {
            projectileCooldownCounter -= Time.deltaTime;
        }
    }

    private void Fly()
    {
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) < 0.1f)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPointIndex].position, speed * Time.deltaTime);
    }

    private void Attack()
    {
        Vector3 direction = playerPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        animator.SetTrigger("castSpell");
        InstantiateProjectile();
    }

    private void InstantiateProjectile()
    {
        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }

    private void Turn()
    {
        isTurning = true;
        rb.velocity = Vector3.zero;
        Quaternion rotation = bodyContainerTransform.transform.rotation;
        rotation.y = isFacingRight ? 0f : 180f;
        bodyContainerTransform.transform.rotation = rotation;
        isFacingRight = !isFacingRight;
        isTurning = false;
    }
    private void TurnHandler()
    {
        Vector3 direction = playerPosition - transform.position;
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
}