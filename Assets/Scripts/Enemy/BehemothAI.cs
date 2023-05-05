using DG.Tweening;
using UnityEngine;

public class BehemothAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform body;

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isTurning;
    [field: SerializeField] public bool IsChargingTowardsPlayer { get; private set; }
    [SerializeField] private bool canKnockPlayer;

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private BehemothStateType currentState;

    private GameObject playerGameObject;
    private Vector2 playerDirection;
    private float rangedDetectionRadius;
    private float meleeDetectionRadius;

    private float timeBetweenKnocks;
    [SerializeField] private float knockCooldownCounter;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangedDetectionRadius);
        Gizmos.DrawWireSphere(transform.position, meleeDetectionRadius);
    }

    private void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        timeBetweenProjectiles = EnemyManager.Instance.BehemothTimeBetweenProjectiles;
        timeBetweenKnocks = EnemyManager.Instance.BehemothTimeBetweenMelee;
        projectilePrefab = EnemyManager.Instance.BehemothProjetilePrefab;
        rangedDetectionRadius = EnemyManager.Instance.BehemothRangedDetectionRadius;
        meleeDetectionRadius = EnemyManager.Instance.BehemothMeleeDetectionRadius;
        projectileCooldownCounter = timeBetweenProjectiles;
        knockCooldownCounter = timeBetweenKnocks;

        currentState = BehemothStateType.Guard;
    }

    private void Update()
    {
        if (playerGameObject == null) return;

        switch (currentState)
        {
            case BehemothStateType.Guard:

                if (IsPlayerInRangedRange())
                {
                    currentState = BehemothStateType.Ranged;
                }
                break;

            case BehemothStateType.Ranged:

                if (!IsPlayerInRangedRange())
                {
                    currentState = BehemothStateType.Guard;
                }
                else if (IsPlayerInMeleeRange())
                {
                    currentState = BehemothStateType.Melee;
                    knockCooldownCounter = timeBetweenKnocks;
                }
                RangedAttack();

                break;

            case BehemothStateType.Melee:

                if (!IsPlayerInMeleeRange())
                {
                    currentState = BehemothStateType.Ranged;
                    knockCooldownCounter = timeBetweenKnocks;
                }
                if (IsPlayerInMeleeRange())
                {
                    KnockHandler();

                    if (IsPlayerInMeleeRange() && canKnockPlayer)
                    {
                        KnockPlayer();
                    }
                }
                break;

            default:
                break;
        }

        ProjectileCooldownHandler();
        TurnHandler();
    }

    private void RangedAttack()
    {
        if (projectileCooldownCounter <= 0)
        {
            InstantiateProjectile();
            projectileCooldownCounter = timeBetweenProjectiles;
        }
    }

    private void KnockHandler()
    {
        if (knockCooldownCounter > 0)
        {
            knockCooldownCounter -= Time.deltaTime;
            if (knockCooldownCounter <= 0)
            {
                canKnockPlayer = true;
            }
            else
            {
                canKnockPlayer = false;
            }
        }
    }

    private void KnockPlayer()
    {
        if (playerGameObject == null) return;

        PlayerController.Instance.Knockback(playerDirection, 500, 30, 500);
        knockCooldownCounter = timeBetweenKnocks;
        canKnockPlayer = false;
    }

    private void ProjectileCooldownHandler()
    {
        if (projectileCooldownCounter > 0)
        {
            projectileCooldownCounter -= Time.deltaTime;
        }
    }

    private bool IsPlayerInRangedRange()
    {
        return GetDistanceFromPlayer() <= rangedDetectionRadius;
    }

    private bool IsPlayerInMeleeRange()
    {
        return GetDistanceFromPlayer() <= meleeDetectionRadius;
    }

    private float GetDistanceFromPlayer()
    {
        if (playerGameObject == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, playerGameObject.transform.position);
    }

    private void Turn()
    {
        isTurning = true;
        rb.velocity = Vector3.zero;
        Quaternion rotation = body.transform.rotation;
        rotation.y = isFacingRight ? 180f : 0f;
        body.transform.rotation = rotation;
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

    private void InstantiateProjectile()
    {
        Instantiate(projectilePrefab, projectilePointTransform.position, Quaternion.identity);
    }
}
