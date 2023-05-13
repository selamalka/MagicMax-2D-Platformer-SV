using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public async void StartGame()
    {
        GameObject.Find("Background").GetComponent<SpriteRenderer>().DOColor(Color.black, 2);
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
