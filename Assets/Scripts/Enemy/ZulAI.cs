using System;
using UnityEngine;

public class ZulAI : MonoBehaviour
{
    [SerializeField] private Transform bodyContainerTransform;
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolPointIndex;
    private ZulStateType currentState;
    private Vector3 playerPosition;
    private bool isTurning;
    private bool isFacingRight;
    private Rigidbody2D rb;

    private float speed;
    private float currentLerpValue;
    private float targetLerpValue;



    private void Start()
    {
        playerPosition = PlayerController.Instance.transform.position;
        rb = GetComponent<Rigidbody2D>();
        currentState = ZulStateType.Fly;
        speed = EnemyManager.Instance.ZulSpeed;
    }

    private void Update()
    {
        playerPosition = PlayerController.Instance.transform.position;

        TurnHandler();

        switch (currentState)
        {
            case ZulStateType.Fly:
                Fly();
                break;

            case ZulStateType.Attack:
                break;

            case ZulStateType.Summon:
                break;

            default:
                break;
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