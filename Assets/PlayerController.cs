using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float accelerationTime = 0.2f;
    [SerializeField] private float deceleationTime = 0.2f;
    [SerializeField] private float maxSpeed = 5f;
    private float currentSpeed;
    private float velocityXSmoothing;

    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    [SerializeField] private bool isGrounded;
    private float inputX;
    private bool isJumpPressed;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);

        inputX = Input.GetAxisRaw("Horizontal");
        isJumpPressed = isGrounded ? Input.GetKeyDown(KeyCode.Space) : false;

        if (isJumpPressed)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        float targetVelocityX = maxSpeed * inputX;
        currentSpeed = Mathf.SmoothDamp(rb.velocity.x, targetVelocityX, ref velocityXSmoothing, inputX > 0 ? accelerationTime : deceleationTime);
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }
}
