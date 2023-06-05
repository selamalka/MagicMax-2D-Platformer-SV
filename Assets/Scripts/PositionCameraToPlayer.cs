using UltimateCameraController.Cameras.Controllers;
using UnityEngine;

public class PositionCameraToPlayer : MonoBehaviour
{
    private void Awake()
    {
/*        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            transform.position = new Vector3(115f, -5.6f, -10f); 
        }
        else
        {
            transform.position = new Vector3(9.7f, -4.2f, -10f);
        }*/
    }

    private void Start()
    {
        SetCameraToPlayer();
    }

    private void Update()
    {
        if (GameObject.FindWithTag("Player") == null) return;
        transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(12f, 8.4f, -10f);
    }

    public void SetCameraToPlayer()
    {
        GetComponent<CameraController>().targetObject = GameObject.FindWithTag("Player").transform;

/*        if (ProgressionManager.Instance.IsTesting || ProgressionManager.Instance.IsCheckpointSaved || SceneManager.GetActiveScene().buildIndex == 4)
        {
            transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(12f, 8.4f, -10f);
        }*/
    }
}