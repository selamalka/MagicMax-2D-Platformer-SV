using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;
    [SerializeField] private GameObject dustCloudPrefab;
    [SerializeField] private Material whiteFlashMaterial;

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateDustCloud(Transform transform)
    {
        Instantiate(dustCloudPrefab, transform.position, Quaternion.identity);
    }
}
