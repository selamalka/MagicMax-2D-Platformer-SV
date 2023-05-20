using DG.Tweening;
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
    [SerializeField] private int extraJumps;
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
    private bool isZooming;

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
        extraJumps = 1;
        ghostScript = GetComponent<Ghost>();
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!IsControllable) return;
        FacingHandler();
        IsGroundedHandler();
        ZoomHandler();
        if (IsNimbusActive) return;
        JumpHandler();
        DashHandler();
    }
    private void FixedUpdate()
    {
        if (!IsControllable) return;
        if (isZooming) return;

        inputX = Input.GetAxisRaw("Horizontal");

        if (Rb.velocity.y < 0)
        {
            Rb.velocity -= gravityVector * fallMultiplier;
        }

        if (IsNimbusActive) return;

        Rb.velocity = new Vector2(inputX * speed, Rb.velocity.y);
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
        if (Rb == null) return;

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

        if (collision.gameObject.CompareTag("Ground"))
        {
            Animator.SetTrigger("land");

            if (IsGrounded)
            {
                FXManager.Instance.InstantiateDustCloud(groundCheckTransform);
                FXManager.Instance.CameraShaker.Shake(FXManager.Instance.PlayerLandShakePreset);
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
                dashDirection = IsFacingRight ? Vector2.right : Vector2.left;
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
            extraJumps = 1;
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
            jumpTimeCounter = jumpTime;
            SetGravityScale(2);
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
            extraJumps--;
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !IsGrounded)
        {
            Animator.SetTrigger("jump");
            Rb.velocity = new Vector2(Rb.velocity.x, 0);
            Rb.velocity = new Vector2(Rb.velocity.x, jumpForce * 2.5f);
            extraJumps--;
            FXManager.Instance.InstantiateDustCloud(groundCheckTransform);
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
            Rb.velocity = new Vector2(Rb.velocity.x, upPushForce);
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
            print("suppose to push left");
            Rb.velocity = Vector2.left * sidePushForce;
        }
        else if (enemyDirection.x < 0)
        {
            print("suppose to push right");
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
    private void ZoomHandler()
    {
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (Input.GetKey(KeyCode.Z))
        {
            isZooming = true;
            Animator.SetBool("isWalking", false);
            Rb.velocity = new Vector2(0, Rb.velocity.y);

            if (cam.orthographicSize < 25)
            {
                cam.orthographicSize += 0.075f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            isZooming = false;
            cam.DOOrthoSize(16, 0.6f);
        }
    }
}