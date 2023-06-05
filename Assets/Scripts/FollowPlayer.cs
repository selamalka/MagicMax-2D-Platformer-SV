
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private void Update()
    {
        if (PlayerController.Instance == null) return;
        transform.position = PlayerController.Instance.transform.position + new Vector3(12f, 8.4f, -10f);
    }
}