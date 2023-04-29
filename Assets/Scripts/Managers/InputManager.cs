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

        if (IsQPressed())
        {
            EventManager.OnQPressed?.Invoke();
        }

        if (IsWPressed())
        {
            EventManager.OnWPressed?.Invoke();
        }

        if (IsEPressed())
        {
            EventManager.OnEPressed?.Invoke();
        }

        if (IsTPressed())
        {
            EventManager.OnTPressed?.Invoke();
        }
    }

    private bool IsQPressed()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }

    private bool IsWPressed()
    {
        return Input.GetKeyDown(KeyCode.W);
    }

    private bool IsEPressed()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public bool IsTPressed()
    {
        return Input.GetKeyDown(KeyCode.T);
    }

    public bool IsFPressed()
    {
        return Input.GetKey(KeyCode.F);
    }
}
