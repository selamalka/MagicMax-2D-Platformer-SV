using UnityEngine;

public class BehemothAI : MonoBehaviour
{
    [SerializeField] private Transform projectilePointTransform;
    [SerializeField] private Transform body;

    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private bool isTurning;

    private GameObject projectilePrefab;
    private float timeBetweenProjectiles;
    private float projectileCooldownCounter;
    private BehemothStateType currentState;

    private GameObject playerGameObject;
    private float detectionRadius;

    private void Start()
    {
        playerGameObject = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        speed = EnemyManager.Instance.BehemothSpeed;
        timeBetweenProjectiles = EnemyManager.Instance.BehemothTimeBetweenProjectiles;
        projectilePrefab = EnemyManager.Instance.BehemothProjetilePrefab;
        detectionRadius = EnemyManager.Instance.BehemothDetectionRadius;
        projectileCooldownCounter = timeBetweenProjectiles;

        currentState = BehemothStateType.Guard;
    }

    private void Update()
    {
        if (playerGameObject == null) return;

        switch (currentState)
        {
            case BehemothStateType.Guard:
                
                if (IsPlayerInAttackRange())
                {
                    currentState = BehemothStateType.Attack;
                }

                break;

            case BehemothStateType.Attack:

                if (!IsPlayerInAttackRange())
                {
                    currentState = BehemothStateType.Guard;
                }

                //AttackPlayer();
                TurnHandler();
                break;

            default:
                break;
        }

        ProjectileCooldownHandler();
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
        rb.velocity = Vector3.zero;
        Vector3 scale = body.transform.localScale;
        scale.x *= -1;
        body.transform.localScale = scale;
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
