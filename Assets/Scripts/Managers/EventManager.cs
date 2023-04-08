using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void LeftMousePressedAction();
    public static LeftMousePressedAction OnLeftMousePressed;

    public delegate void EPressedAction();
    public static EPressedAction OnEPressed;
}
