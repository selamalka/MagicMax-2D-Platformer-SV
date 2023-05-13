using UnityEngine;

public class MeleeContainer : MonoBehaviour
{
    [SerializeField] private GameObject meleeSlash;

    private void Start()
    {
        transform.localScale = new Vector3(PlayerController.Instance.IsFacingRight ? 1 : -1, 1, 1);
    }

    private void Update()
    {
        if (meleeSlash == null) Destroy(gameObject, 1);
    }
}
