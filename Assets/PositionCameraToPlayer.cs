using UltimateCameraController.Cameras.Controllers;
using UnityEngine;

public class PositionCameraToPlayer : MonoBehaviour
{
    private void Start()
    {
        SetCameraToPlayer();
        transform.position = new Vector3(115f, -5.6f, -10f);
    }

    public void SetCameraToPlayer()
    {
        GetComponent<CameraController>().targetObject = GameObject.FindWithTag("Player").transform;
    }
}