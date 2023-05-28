using UltimateCameraController.Cameras.Controllers;
using UnityEngine;

public class PositionCameraToPlayer : MonoBehaviour
{
    private void Awake()
    {
        transform.position = new Vector3(115f, -5.6f, -10f);
    }

    private void Start()
    {
        SetCameraToPlayer();
    }

    public void SetCameraToPlayer()
    {
        GetComponent<CameraController>().targetObject = GameObject.FindWithTag("Player").transform;

        if (ProgressionManager.Instance.IsTesting || ProgressionManager.Instance.IsCheckpointSaved)
        {
            transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(12f, 8.4f, -10f);
        }
    }
}