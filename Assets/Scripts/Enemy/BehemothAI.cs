using DG.Tweening;
using System;
using UnityEngine;

public class BehemothAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform body;

    private Rigidbody2D rb;
    private Vector3 startingPosition;
    private float speed;
    private bool isFacingRight = true;
    private bool isTurning;
    public bool IsChargingTowardsPlayer { get; private set; }

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private BehemothStateType currentState;

    private GameObject playerGameObject;
    private Vector2 playerDirection;
    private float rangedDetectionRadius;
    private float meleeDetectionRadius;

    private float timeBetweenMelee;
    private float meleeCooldownCounter;

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
        speed = EnemyManager.Instance.BehemothSpeed;
        timeBetweenProjectiles = EnemyManager.Instance.BehemothTimeBetweenProjectiles;
        timeBetweenMelee = EnemyManager.Instance.BehemothTimeBetweenMelee;
        projectilePrefab = EnemyManager.Instance.BehemothProjetilePrefab;
        rangedDetectionRadius = EnemyManager.Instance.BehemothRangedDetectionRadius;
        meleeDetectionRadius = EnemyManager.Instance.BehemothMeleeDetectionRadius;
        projectileCooldownCounter = timeBetweenProjectiles;
        meleeCooldownCounter = timeBetweenMelee;

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

                TurnHandler();
                break;

            case BehemothStateType.Ranged:

                if (!IsPlayerInRangedRange())
                {
                    currentState = BehemothStateType.Guard;
                }
                else if (IsPlayerInMeleeRange())
                {
                    currentState = BehemothStateType.Melee;
                }
                RangedAttack();
                TurnHandler();
                break;

            case BehemothStateType.Melee:

                if (!IsPlayerInMeleeRange())
                {
                    currentState = BehemothStateType.Ranged;
                }
                MeleeAttack();
                TurnHandler();
                break;

            default:
                break;
        }

        ProjectileCooldownHandler();
    }

    private void FixedUpdate()
    {
        if (meleeCooldownCounter <= 0)
        {
            ChargePlayer();
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

    private void MeleeAttack()
    {
        meleeCooldownCounter -= Time.deltaTime;

        if (meleeCooldownCounter <= 0)
        {
            // ChargePlayer is in FixedUpdate           
        }
    }

    private void ChargePlayer()
    {
        IsChargingTowardsPlayer = true;
        playerDirection = playerGameObject.transform.position - transform.position;
        rb.velocity = new Vector2(playerDirection.x, playerDirection.y) * Time.deltaTime * 300;
        meleeCooldownCounter = timeBetweenMelee;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsChargingTowardsPlayer = false;
            rb.velocity = Vector2.zero;
            rb.DOMoveX(startingPosition.x, 1f).SetLoops(0).SetEase(Ease.OutQuart);
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
