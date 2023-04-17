using UnityEngine;

public class ShadowDemonAI : MonoBehaviour
{
    private ShadowDemonStateType currentState;
    [SerializeField] private Transform[] patrolPoints;
    private Rigidbody2D rb;
    private float speed;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = EnemyManager.Instance.ShadowDemonSpeed;

        currentState = ShadowDemonStateType.Patrol;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case ShadowDemonStateType.Patrol:
                Move();
                break;

            case ShadowDemonStateType.Attack:
                break;

            default:
                break;
        }
    }

    private void Move()
    {
        
    }
}
