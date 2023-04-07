using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform projectileOrigin;
    [SerializeField] private Transform meleeInstancesParent;
    [SerializeField] private GameObject magicShotPrefab;
    [SerializeField] private GameObject meleeAttackPrefab;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        //EventManager.OnLeftMousePressed += MeleeAttack;
        EventManager.OnLeftMousePressed += FireProjectile;
    }

    private void OnDisable()
    {
        //EventManager.OnLeftMousePressed -= MeleeAttack;
        EventManager.OnLeftMousePressed -= FireProjectile;
    }

    private void FireProjectile()
    {
        var direction = InputManager.Instance.MousePosition - rb.position;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Instantiate(magicShotPrefab, projectileOrigin.position, Quaternion.Euler(new Vector3(0,0,-angle)));
    }

    private void MeleeAttack()
    {
        Instantiate(meleeAttackPrefab, projectileOrigin.position, Quaternion.identity, meleeInstancesParent);
    }
}
