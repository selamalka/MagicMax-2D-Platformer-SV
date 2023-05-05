using DG.Tweening;
using UnityEngine;

public class BehemothAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform body;

    private Rigidbody2D rb;
    private Vector3 startingPosition;
    private bool isFacingRight = true;
    private bool isTurning;
    [field: SerializeField] public bool IsChargingTowardsPlayer { get; private set; }
    [SerializeField] private bool canChargeTowardsPlayer;

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private BehemothStateType currentState;

    private GameObject playerGameObject;
    private Vector2 playerDirection;
    private float rangedDetectionRadius;
    private float meleeDetectionRadius;

    private float timeBetweenCharges;
    [SerializeField] private float chargeCooldownCounter;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangedDetectionRadius);
        Gizmos.DrawWireSphere(transform.position, meleeDetectionRadius);
    }

    private void Start()
    {
        startingPosition = transform.position;
        playerGameObject = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        timeBetweenProjectiles = EnemyManager.Instance.BehemothTimeBetweenProjectiles;
        timeBetweenCharges = EnemyManager.Instance.BehemothTimeBetweenMelee;
        projectilePrefab = EnemyManager.Instance.BehemothProjetilePrefab;
        rangedDetectionRadius = EnemyManager.Instance.BehemothRangedDetectionRadius;
        meleeDetectionRadius = EnemyManager.Instance.BehemothMeleeDetectionRadius;
        projectileCooldownCounter = timeBetweenProjectiles;
        chargeCooldownCounter = timeBetweenCharges;

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
                    rb.DOMoveX(startingPosition.x, 0.5f).SetLoops(0);
                }
                else if (IsPlayerInMeleeRange())
                {
                    currentState = BehemothStateType.Melee;
                    chargeCooldownCounter = timeBetweenCharges;
                }
                rb.velocity = Vector2.zero;
                RangedAttack();

                break;

            case BehemothStateType.Melee:

                if (!IsPlayerInMeleeRange())
                {
                    currentState = BehemothStateType.Ranged;
                    IsChargingTowardsPlayer = false;
                    rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                    rb.DOMoveX(startingPosition.x, 0.5f).SetLoops(0);
                    chargeCooldownCounter = timeBetweenCharges;
                }
                if (IsPlayerInMeleeRange())
                {
                    MeleeHandler();
                }
                break;

            default:
                break;
        }

        ProjectileCooldownHandler();
        TurnHandler();
    }

    private void FixedUpdate()
    {
        if (playerGameObject == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (IsPlayerInMeleeRange() && canChargeTowardsPlayer)
        {
            ChargePlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsChargingTowardsPlayer)
            {
                chargeCooldownCounter = timeBetweenCharges;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!IsChargingTowardsPlayer)
            {
                rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }

    private void RangedAttack()
    {
        if (projectileCooldownCounter <= 0)
        {
            InstantiateProjectile();
            projectileCooldownCounter = timeBetweenProjectiles;
        }
    }

    private void MeleeHandler()
    {
        if (chargeCooldownCounter > 0)
        {
            chargeCooldownCounter -= Time.deltaTime;
            if (chargeCooldownCounter <= 0)
            {
                canChargeTowardsPlayer = true;
            }
            else
            {
                canChargeTowardsPlayer = false;
            }
        }
    }

    private void ChargePlayer()
    {
        if (playerGameObject == null) return;

        IsChargingTowardsPlayer = true;
        playerDirection = (playerGameObject.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(playerDirection.x, playerDirection.y) * 60;

        if (Vector2.Distance(transform.position, playerGameObject.transform.position) <= 10f)
        {
            rb.velocity = Vector2.zero;
            PlayerController.Instance.Knockback(-playerDirection, 50, 20, 500);
            rb.DOMoveX(startingPosition.x, 0.5f).SetLoops(0);
            chargeCooldownCounter = timeBetweenCharges;
            IsChargingTowardsPlayer = false;
            canChargeTowardsPlayer = false;
        }
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
