using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsPaused { get; private set; }
    [field: SerializeField] public float PauseTime { get; private set; }

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

    public IEnumerator PauseGameEffect(float pauseTime)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(PauseTime);
        Time.timeScale = 1f;
    }
}
