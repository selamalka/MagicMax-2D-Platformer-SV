using System.Threading.Tasks;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;
    [SerializeField] private GameObject dustCloudPrefab;
    [SerializeField] private Material whiteFlashMaterial;
    private Material originalMaterial;

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateDustCloud(Transform transform)
    {
        Instantiate(dustCloudPrefab, transform.position, Quaternion.identity);
    }

    public async void FlashWhite(GameObject objectToFlash)
    {
        SpriteRenderer[] spriteRenderers = objectToFlash.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            originalMaterial = renderer.sharedMaterial;
            renderer.sharedMaterial = whiteFlashMaterial;
        }

        await Task.Delay(150);

        foreach (SpriteRenderer renderer2 in spriteRenderers)
        {
            if (renderer2 != null)
            {
                renderer2.sharedMaterial = originalMaterial;
            }
        }
    }
}
