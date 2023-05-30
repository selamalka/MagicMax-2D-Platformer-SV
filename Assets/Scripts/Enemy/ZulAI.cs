using System;
using UnityEngine;

public class ZulAI : MonoBehaviour
{
    [SerializeField] private Transform bodyContainerTransform;
    private Vector3 leftFlyingBorder;
    private Vector3 rightFlyingBorder;
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
        if (currentLerpValue == targetLerpValue)
        {
            targetLerpValue = targetLerpValue == 0 ? 1 : 0;
        }

        currentLerpValue = Mathf.MoveTowards(currentLerpValue, targetLerpValue, speed * Time.deltaTime);

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
        Vector3 aboveThePlayerPosition = playerPosition + new Vector3(0, 6, 0);
        transform.position = aboveThePlayerPosition;
        leftFlyingBorder = aboveThePlayerPosition + new Vector3(-5, 0, 0);
        rightFlyingBorder = aboveThePlayerPosition + new Vector3(5, 0, 0);


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
        Vector3 direction = PlayerController.Instance.gameObject.transform.position - transform.position;
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