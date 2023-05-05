using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EPressedAction();
    public static EPressedAction OnEPressed;

/*    public delegate void WPressedAction();
    public static WPressedAction OnWPressed;*/

    public delegate void QPressedAction();
    public static QPressedAction OnQPressed;

    public delegate void TPressedAction();
    public static TPressedAction OnTPressed;

    public delegate void PlayerGetHitAction();
    public static PlayerGetHitAction OnPlayerGetHit;

    public delegate void EnemyDeathAction();
    public static EnemyDeathAction OnEnemyDeath;

    public delegate void NimbusIsActiveAction();
    public static NimbusIsActiveAction OnNimbusIsActive;

    public delegate void PlayerLevelUpAction();
    public static PlayerLevelUpAction OnPlayerLevelUp;
}
