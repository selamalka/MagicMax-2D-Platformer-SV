using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void SPressedAction();
    public static SPressedAction OnSPressed;

    public delegate void EscPressedAction();
    public static EscPressedAction OnEscPressed;

    public delegate void PlayerGetHitAction();
    public static PlayerGetHitAction OnPlayerGetHit;

    public delegate void EnemyDeathAction();
    public static EnemyDeathAction OnEnemyDeath;

    public delegate void NimbusIsActiveAction();
    public static NimbusIsActiveAction OnNimbusIsActive;

    public delegate void PlayerLevelUpAction();
    public static PlayerLevelUpAction OnPlayerLevelUp;
}
