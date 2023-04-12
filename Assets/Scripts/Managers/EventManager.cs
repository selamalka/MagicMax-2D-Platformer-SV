using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void LeftMousePressedAction();
    public static LeftMousePressedAction OnLeftMousePressed;

    public delegate void EPressedAction();
    public static EPressedAction OnEPressed;

    public delegate void QPressedAction();
    public static QPressedAction OnQPressed;

    public delegate void PlayerGetHitAction();
    public static PlayerGetHitAction OnPlayerGetHit;

    public delegate void PlayerUseManaAction(float manaCost);
    public static PlayerUseManaAction OnPlayerUseMana;

    public delegate void EnemyDeathAction();
    public static EnemyDeathAction OnEnemyDeath;

}
