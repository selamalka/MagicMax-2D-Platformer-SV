using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private float speed;
    [SerializeField] private float fallMultiplier;

    [SerializeField] private GameObject body;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius;
    [field: SerializeField] public bool IsGrounded { get; private set; }
    public bool IsFacingRight { get; private set; }
    private bool isJumping;
    private float jumpTimeCounter;
    private Vector2 gravityVector;
    private float inputX;
    public bool IsControllable { get; private set; }
    public bool IsNimbusActive { get; private set; }

    [Header("Dashing")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private Vector2 dashDirection;
    private bool canDash = true;
    private bool isDashing;

    private Ghost ghostScript;
    private Rigidbody2D rb;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsControllable = true;
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        IsFacingRight = true;
        ghostScript = GetComponent<Ghost>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsControllable) return;
        FacingHandler();

        IsGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);

        JumpHandler();

        if (rb.velocity.y < 0)
        {
            rb.velocity -= gravityVector * fallMultiplier * Time.deltaTime;
        }

        if (IsNimbusActive) return;
        var isDashPressed = Input.GetKeyDown(KeyCode.LeftShift);
        Dash(isDashPressed);
    }

    private void FixedUpdate()
    {
        if (!IsControllable) return;

        inputX = Input.GetAxisRaw("Horizontal");

        if (IsNimbusActive) return;

        rb.velocity = new Vector2(inputX * speed * Time.deltaTime, rb.velocity.y);

        if (isDashing)
        {
            rb.velocity = dashDirection.normalized * dashForce;
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nimbus"))
        {
            SetGravityScale(0);
            rb.mass = 0f; 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nimbus"))
        {
            Vector2 nimbusVelocity = FindObjectOfType<Nimbus>().GetComponent<Rigidbody2D>().velocity;
            rb.velocity = nimbusVelocity;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nimbus"))
        {
            SetGravityScale(8);
            rb.mass = 1f; 
        }
    }


    public void SetIsCloudActive(bool value)
    {
        IsNimbusActive = value;
    }

    public void SetIsControllable(bool value)
    {
        IsControllable = value;
    }

    private void JumpHandler()
    {
        if (IsNimbusActive) return;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            SetGravityScale(2);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(inputX, jumpForce * 2f);
                jumpTimeCounter -= Time.deltaTime;
                jumpTimeCounter = jumpTimeCounter < 0 ? 0 : jumpTimeCounter;
            }
            else
            {
                isJumping = false;
                SetGravityScale(8);
                rb.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimeCounter = 0;
            isJumping = false;
            SetGravityScale(8);
            rb.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
        }
    }

    private void Dash(bool isDashPressed)
    {
        if (isDashPressed && canDash)
        {
            isDashing = true;
            canDash = false;
            ghostScript.SetShouldCreateGhost(true);

            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(body.transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        ghostScript.SetShouldCreateGhost(false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void SetGravityScale(float value)
    {
        rb.gravityScale = value;
    }

    private void Turn()
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
            Turn();
        }
        else if (inputX > 0 && !IsFacingRight)
        {
            Turn();
        }
    }
}
