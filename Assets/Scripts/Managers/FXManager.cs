using System.Threading.Tasks;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;
    [SerializeField] private GameObject dustCloudPrefab;
    [SerializeField] private Material whiteFlashMaterial;
    private Material originalMaterial;

    [SerializeField] private float pauseGameCooldownTime;
    private float pauseGameCounter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pauseGameCounter = pauseGameCooldownTime;
    }

    private void Update()
    {
        PauseGameHandler();
    }

    private void PauseGameHandler()
    {
        if (pauseGameCounter > 0)
        {
            pauseGameCounter -= Time.deltaTime;
        }
        else
        {
            pauseGameCounter = 0;
        }
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

    public async void PauseGameEffect(int pauseTimeInMilliseconds)
    {
        if (pauseGameCounter <= 0)
        {
            Time.timeScale = 0.05f;
            await Task.Delay(pauseTimeInMilliseconds);
            Time.timeScale = 1f;
            pauseGameCounter = pauseGameCounter = pauseGameCooldownTime;
        }
    }
}
