using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float jumpForce;
    [SerializeField] private float accelerationTime = 0.2f;
    [SerializeField] private float deceleationTime = 0.2f;
    [SerializeField] private float maxSpeed = 5f;

    private float currentSpeed;
    private float currentVelocityXSmoothing;

    [SerializeField] private GameObject body;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    [SerializeField] private bool isGrounded;
    private float inputX;
    private float inputY;
    private bool isJumpPressed;
    public bool IsFacingRight { get; private set; }

    private Rigidbody2D rb;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsFacingRight = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        isJumpPressed = isGrounded ? Input.GetKeyDown(KeyCode.Space) : false;

        FacingHandler();

        if (isJumpPressed && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        float targetVelocityX = maxSpeed * inputX;
        currentSpeed = Mathf.SmoothDamp(rb.velocity.x, targetVelocityX, ref currentVelocityXSmoothing, inputX > 0 ? accelerationTime : deceleationTime);

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    private void SetGravityScale(float value)
    {
        rb.gravityScale = value;
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        Vector3 localScale = body.transform.localScale;
        localScale.x *= -1;
        body.transform.localScale = localScale;

        IsFacingRight = !IsFacingRight;
    }

    private void FacingHandler()
    {
        if (inputX < 0 && IsFacingRight)
        {
            Flip();
        }
        else if (inputX > 0 && !IsFacingRight)
        {
            Flip();
        }
    }
}
