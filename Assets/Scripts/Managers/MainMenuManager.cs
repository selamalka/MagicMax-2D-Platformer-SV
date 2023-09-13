using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

    private void Start()
    {
        Cursor.visible = true;
        backgroundSpriteRenderer = GameObject.Find("Background").GetComponent<SpriteRenderer>();
    }

    public async void StartGame()
    {
        backgroundSpriteRenderer.DOColor(Color.black, 2);
        GameObject.Find("Canvas").SetActive(false);
        await Task.Delay(2000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }
}
