using UnityEngine;

public class Nimbus : MonoBehaviour
{
    [field: SerializeField] public SpellData SpellData { get; private set; }

    private float lifespanCounter;
    public Rigidbody2D Rb { get; private set; }
    [SerializeField] private float speed;
    private float inputY;
    private float inputX;
    private Vector3 playerPosition;

    private void Start()
    {
        PlayerController.Instance.SetIsCloudActive(true);
        playerPosition = GameObject.FindWithTag("Player").transform.position;
        transform.position = playerPosition + new Vector3(0,-2,0);
        Rb = GetComponent<Rigidbody2D>();
        lifespanCounter = SpellData.Lifespan;
    }

    private void Update()
    {
        LifespanHandler();
    }

    private void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        Rb.velocity = new Vector2(inputX * speed * Time.deltaTime, inputY * speed * Time.deltaTime);
    }

    private void LifespanHandler()
    {
        lifespanCounter -= Time.deltaTime;

        if (lifespanCounter <= 0)
        {
            PlayerController.Instance.SetIsCloudActive(false);
            Destroy(gameObject);
        }
    }
}
