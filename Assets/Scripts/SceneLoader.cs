using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool IsKeyCollected { get; private set; } = false;

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsKeyCollected)
        {
            UIManager.Instance.SetKeyDisplay(false);
            UIManager.Instance.FadeToBlack(2);
            await Task.Delay(2000);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            print("You require a key");
        }
    }

    public void SetIsKeyCollected(bool value)
    {
        IsKeyCollected = value;
    }
}
