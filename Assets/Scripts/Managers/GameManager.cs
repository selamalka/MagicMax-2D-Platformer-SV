using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        IsPaused = false;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        IsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        IsPaused = false;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
        DOTween.KillAll();
    }
}