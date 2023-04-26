using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;
    [SerializeField] private GameObject dustCloudPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateDustCloud(Transform transform)
    {
        Instantiate(dustCloudPrefab, transform.position, Quaternion.identity);
    }
}
