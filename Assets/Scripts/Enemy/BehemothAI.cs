using DG.Tweening;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    [field: SerializeField] public bool IsChargingTowardsPlayer { get; private set; }
    [SerializeField] private bool canChargeTowardsPlayer;
    private bool isPlayerHit;

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private BehemothStateType currentState;

    private GameObject playerGameObject;
    private Vector2 playerDirection;
    private float rangedDetectionRadius;
    private float meleeDetectionRadius;

    private float timeBetweenMelee;
    [SerializeField] private float meleeCooldownCounter;

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
                    meleeCooldownCounter = timeBetweenMelee;
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
                    meleeCooldownCounter = timeBetweenMelee;
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
        if (IsPlayerInMeleeRange() && canChargeTowardsPlayer)
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

    private void MeleeHandler()
    {
        if (meleeCooldownCounter > 0)
        {
            meleeCooldownCounter -= Time.deltaTime;
            if (meleeCooldownCounter <= 0)
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
        IsChargingTowardsPlayer = true;
        playerDirection = (playerGameObject.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(playerDirection.x, playerDirection.y) * Time.deltaTime * 800;

        if (isPlayerHit)
        {
            print(isPlayerHit);
            isPlayerHit = false;
            IsChargingTowardsPlayer = false;
            meleeCooldownCounter = timeBetweenMelee;
        }
        else
        {
            IsChargingTowardsPlayer = false;
            PlayerController.Instance.Knockback(-playerDirection, 30, 20, 700);
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
                isPlayerHit = true;
                meleeCooldownCounter = timeBetweenMelee;
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
