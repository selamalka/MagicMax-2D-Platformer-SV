using System;
using System.Data;
using UnityEngine;

public class ImpAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;
    private Vector3 playerPosition;
    private Vector3 startingPosition;
    private Vector3 targetTravelPosition;
    [SerializeField] private Vector3 travelDistance = new Vector3(5, 0, 0);
    [SerializeField] private Transform shotPointTransform;
    private GameObject projectilePrefab;
    private float timeBetweenShots;
    private float shotCooldownCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        targetTravelPosition = startingPosition + travelDistance;

        speed = EnemyManager.Instance.ImpSpeed;
        timeBetweenShots = EnemyManager.Instance.ImpTimeBetweenShots;
        projectilePrefab = EnemyManager.Instance.ImpProjetilePrefab;

        shotCooldownCounter = timeBetweenShots;
    }

    private void Update()
    {
        if (shotCooldownCounter > 0)
        {
            shotCooldownCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        TurnHandler();
        Move();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPosition = collision.transform.position;
            InstantiateProjectile(playerPosition);
        }
    }

    private void InstantiateProjectile(Vector3 targetPosition)
    {
        if (projectilePrefab == null) return;
        if (shotCooldownCounter <= 0)
        {
            var direction = targetPosition - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var projectile = Instantiate(projectilePrefab, shotPointTransform.position, Quaternion.Euler(new Vector3(0, 0, angle - 90)));
            projectile.GetComponent<EnemyProjectile>().SetTarget(targetPosition);
            shotCooldownCounter = timeBetweenShots;
        }
    }

    private void TurnHandler()
    {
        if (transform.position.x > targetTravelPosition.x)
        {
            Turn();
        }
        else if (transform.position.x < startingPosition.x)
        {
            Turn();
        }
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

    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
}
