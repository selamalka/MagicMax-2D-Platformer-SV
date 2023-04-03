using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void LeftMousePressedAction();
    public static LeftMousePressedAction OnLeftMousePressed;
}
