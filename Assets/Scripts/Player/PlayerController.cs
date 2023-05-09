using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [field: SerializeField] public bool IsGrounded { get; private set; }
    [field: SerializeField] public bool IsControllable { get; private set; }
    [field: SerializeField] public bool IsKnocked { get; private set; }
    [field: SerializeField] public bool IsFacingRight { get; private set; }
    public bool IsNimbusActive { get; private set; }

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private float speed;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private int sidePushForce;
    [SerializeField] private int upPushForce;
    [SerializeField] private float downPushForce;
    [SerializeField] private GameObject body;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRayLength;

    private bool isJumping;
    [SerializeField] private int numberOfJumps;
    [SerializeField] private bool canDoubleJump;
    private float jumpTimeCounter;
    private Vector2 gravityVector;
    private float inputX;

    [Header("Dashing")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    private Vector2 dashDirection;
    private bool canDash = true;
    private bool isDashing;
    private bool hasDashedAfterGrounded;

    public Animator Animator { get; private set; }
    private Ghost ghostScript;
    public Rigidbody2D Rb { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        EventManager.OnNimbusIsActive += PushPlayerDown;
    }
    private void OnDisable()
    {
        EventManager.OnNimbusIsActive -= PushPlayerDown;
    }
    private void Start()
    {
        IsControllable = true;
        gravityVector = new Vector2(0, -Physics2D.gravity.y);
        IsFacingRight = true;
        canDoubleJump = true;
        numberOfJumps = 2;
        ghostScript = GetComponent<Ghost>();
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!IsControllable) return;
        FacingHandler();
        IsGroundedHandler();

        if (IsNimbusActive) return;
        JumpHandler();
        DashHandler();
    }
    private void FixedUpdate()
    {
        if (!IsControllable) return;

        inputX = Input.GetAxisRaw("Horizontal");

        if (Rb.velocity.y < 0)
        {
            Rb.velocity -= gravityVector * fallMultiplier * Time.deltaTime;
        }

        if (IsNimbusActive) return;

        Rb.velocity = new Vector2(inputX * speed * Time.deltaTime, Rb.velocity.y);
        if (Rb.velocity.x != 0f)
        {
            Animator.SetBool("isWalking", true);
        }
        else
        {
            Animator.SetBool("isWalking", false);
        }
        if (isDashing)
        {
            Rb.velocity = dashDirection.normalized * dashForce;
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nimbus"))
        {
            SetGravityScale(0);
            Rb.mass = 0f;
        }

        if (collision.gameObject.CompareTag("Behemoth"))
        {
            Rb.velocity = Vector2.zero;
            Vector2 collisionDirection = (collision.transform.position - transform.position).normalized;
            Knockback(collisionDirection, 30, 20, 500);
        }

        if (collision.gameObject.CompareTag("Tilemap"))
        {
            Animator.SetTrigger("land");
            numberOfJumps = 2;

            if (IsGrounded)
            {                
                FXManager.Instance.InstantiateDustCloud(groundCheckTransform);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nimbus"))
        {
            Vector2 nimbusVelocity = FindObjectOfType<Nimbus>().GetComponent<Rigidbody2D>().velocity;
            Rb.velocity = nimbusVelocity;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Nimbus"))
        {
            SetGravityScale(8);
            Rb.mass = 1f;
        }
    }

    private void DashHandler()
    {
        bool isDashPressed = Input.GetKeyDown(KeyCode.LeftShift);

        if (IsGrounded)
        {
            hasDashedAfterGrounded = false;

            if (isDashPressed)
            {
                Dash();
                canDoubleJump = true;
            }
        }
        else if (!IsGrounded && !hasDashedAfterGrounded && isDashPressed)
        {
            Dash();
            hasDashedAfterGrounded = true;
        }
    }
    private void Dash()
    {
        if (canDash)
        {
            isDashing = true;
            canDash = false;
            ghostScript.SetShouldCreateGhost(true);

            dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0);
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

    private void IsGroundedHandler()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(groundCheckTransform.position, Vector2.down, groundCheckRayLength, groundLayer);

        if (raycastHit.collider != null)
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
    private void JumpHandler()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Animator.SetTrigger("jump");
            isJumping = true;
            //canDoubleJump = true;
            jumpTimeCounter = jumpTime;
            SetGravityScale(2);
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
            numberOfJumps--;
        }

        if (Input.GetKeyDown(KeyCode.Space) && numberOfJumps > 0 && !IsGrounded /*&& !IsGrounded && canDoubleJump*/)
        {
            Animator.SetTrigger("jump");
            Rb.velocity = new Vector2(Rb.velocity.x, 0);
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce * 2.5f);
            numberOfJumps--;
            FXManager.Instance.InstantiateDustCloud(groundCheckTransform);
            //canDoubleJump = false;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                Rb.velocity = new Vector2(Rb.velocity.x, jumpForce * 2f);
                jumpTimeCounter -= Time.deltaTime;
                jumpTimeCounter = jumpTimeCounter < 0 ? 0 : jumpTimeCounter;
            }
            else
            {
                isJumping = false;
                SetGravityScale(10);
                Rb.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTimeCounter = 0;
            isJumping = false;
            SetGravityScale(8);
            Rb.AddForce(Vector2.down * 10, ForceMode2D.Impulse);
        }
    }

    public async void Knockback(Vector2 collisionDirection, int horizontalForce, int verticalForce, int isControllableDelay)
    {
        IsControllable = false;
        Rb.velocity = Vector2.zero;
        Rb.velocity = new Vector2(-collisionDirection.x * horizontalForce, verticalForce);
        await Task.Delay(isControllableDelay);
        SetGravityScale(8);
        IsControllable = true;
    }
    public async void PushPlayerAgainstEnemyDirectionOnMelee(Vector2 enemyDirection)
    {
        if (!IsGrounded && Input.GetKey(KeyCode.DownArrow) && enemyDirection.y < 0)
        {
            SetGravityScale(2);
            Rb.velocity = new Vector2(Rb.velocity.x, upPushForce * Time.deltaTime);
            await Task.Delay(200);
            SetGravityScale(8);
        }
        else if (!IsGrounded && Input.GetKey(KeyCode.UpArrow) && enemyDirection.y > 0)
        {
            Rb.velocity = Vector2.zero;
            Rb.velocity = new Vector2(0, -downPushForce);
        }

        if (Input.GetKey(KeyCode.UpArrow)) return;

        if (enemyDirection.x > 0)
        {
            Rb.velocity = Vector2.left * sidePushForce;
        }
        else if (enemyDirection.x < 0)
        {
            Rb.velocity = Vector2.right * sidePushForce;
        }
    }
    private void PushPlayerDown()
    {
        Rb.velocity = Vector2.zero;
        Rb.AddForce(Vector2.down * 20);
    }

    private void SetGravityScale(float value)
    {
        if (Rb == null) return;
        Rb.gravityScale = value;
    }
    public void SetIsNimbusActive(bool value)
    {
        IsNimbusActive = value;
    }
    public void SetIsControllable(bool value)
    {
        IsControllable = value;
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
    private void Turn()
    {
        Quaternion rotation = transform.rotation;
        rotation.y = IsFacingRight ? 180f : 0f;
        transform.rotation = rotation;
        IsFacingRight = !IsFacingRight;
    }
}