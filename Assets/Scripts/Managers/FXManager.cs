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
            originalMaterial = renderer.material;
            renderer.material = whiteFlashMaterial;
        }

        await Task.Delay(125);

        foreach (SpriteRenderer renderer2 in spriteRenderers)
        {
            if (renderer2 != null)
            renderer2.material = originalMaterial;
        }
    }
}
