using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

/*    [SerializeField] private Camera cam;
    [SerializeField] private GameObject crosshair;
    public Vector2 MousePosition { get; private set; }*/


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        /*        MousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                crosshair.transform.position = MousePosition;*/
        IsQPressed();
        IsWPressed();
        IsEPressed();

        if (IsSPressed())
        {
            EventManager.OnSPressed?.Invoke();
        }
    }

    public bool IsQPressed()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    public bool IsWPressed()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    public bool IsEPressed()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool IsSPressed()
    {
        return Input.GetKeyDown(KeyCode.S);
    }
}
