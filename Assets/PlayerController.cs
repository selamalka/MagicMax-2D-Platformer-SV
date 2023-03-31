using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float accelerationTime = 0.2f;
    [SerializeField] private float deceleationTime = 0.2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float fallSpeedMultiplier;
    private float currentSpeed;
    private float velocityXSmoothing;

    [SerializeField] private GameObject body;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    [SerializeField] private bool isGrounded;
    private float inputX;
    private float inputY;
    private bool isJumpPressed;
    private bool isFacingRight;

    [SerializeField] private AnimationCurve fallCurve;
    private float time;
    private float curveSpeed;

    private Rigidbody2D rb;

    private void Start()
    {
        isFacingRight = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        isJumpPressed = isGrounded ? Input.GetKeyDown(KeyCode.Space) : false;



        time = Time.deltaTime;
        curveSpeed = fallCurve.Evaluate(time);

        FacingHandler();

        if (isJumpPressed)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        //print(rb.velocity.y);

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
        currentSpeed = Mathf.SmoothDamp(rb.velocity.x, targetVelocityX, ref velocityXSmoothing, inputX > 0 ? accelerationTime : deceleationTime);
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);


        print(inputY);

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

        isFacingRight = !isFacingRight;
    }

    private void FacingHandler()
    {
        if (inputX < 0 && isFacingRight)
        {
            Flip();
        }
        else if (inputX > 0 && !isFacingRight)
        {
            Flip();
        }
    }
}
