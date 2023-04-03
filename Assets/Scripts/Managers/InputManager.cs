using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject crosshair;

    public Vector2 MousePosition { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        crosshair.transform.position = MousePosition;

        if (IsLeftMousePressed())
        {
            EventManager.OnLeftMousePressed?.Invoke();
        }
    }

    private bool IsLeftMousePressed()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
