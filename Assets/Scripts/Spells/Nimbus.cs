using UnityEngine;

public class Nimbus : MonoBehaviour
{
    [field: SerializeField] public SpellData SpellData { get; private set; }
    [SerializeField] private float speed;
    public Rigidbody2D Rb { get; private set; }

    private Transform spellContainerParent;
    private float lifespanCounter;
    private float inputY;
    private float inputX;
    private bool isPlayerMounted;

    private void Awake()
    {
        if (FindObjectsOfType<Nimbus>().Length >= 2)
        {
            Destroy(gameObject);
        }

        if (PlayerController.Instance.IsGrounded)
        {
            print("Cannot cast Nimbus while grounded");
            Destroy(gameObject);
        }

        GetComponentInParent<Transform>().rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void Start()
    {
        EventManager.OnNimbusIsActive?.Invoke();

        PlayerController.Instance.SetIsNimbusActive(true);
        PlayerStatsManager.Instance.UseMana(1);
        Rb = GetComponent<Rigidbody2D>();

        spellContainerParent = PlayerCombat.Instance.NimbusInstancesParent;
        transform.parent.SetParent(spellContainerParent);
        spellContainerParent.GetComponentInChildren<SpellContainer>().transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = spellContainerParent.transform.position;

        lifespanCounter = SpellData.Lifespan;
    }

    private void Update()
    {
        LifespanHandler();
        GetComponentInParent<Transform>().rotation = Quaternion.Euler(0, GetComponentInParent<Transform>().rotation.y, 0);
    }

    private void FixedUpdate()
    {
        if (!isPlayerMounted) return;
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        Rb.velocity = new Vector2(inputX * speed, inputY * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerMounted = true;
        }
    }

    private void LifespanHandler()
    {
        lifespanCounter -= Time.deltaTime;

        if (lifespanCounter <= 0)
        {
            PlayerController.Instance.SetIsNimbusActive(false);
            Destroy(gameObject);
        }
    }
}