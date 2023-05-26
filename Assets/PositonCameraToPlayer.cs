using UltimateCameraController.Cameras.Controllers;
using UnityEngine;

public class PositonCameraToPlayer : MonoBehaviour
{
    void Start()
    {
        SetCameraToPlayer();
    }

    public void SetCameraToPlayer()
    {
        GetComponent<CameraController>().targetObject = GameObject.Find("Player").transform;
        transform.position = GameObject.Find("Player").transform.position;
        transform.position += new Vector3(7.5f, 6.5f, -10);
    }
}
