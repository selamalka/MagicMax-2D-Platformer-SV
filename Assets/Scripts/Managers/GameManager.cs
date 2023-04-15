using UnityEngine;

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
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        IsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }
}
