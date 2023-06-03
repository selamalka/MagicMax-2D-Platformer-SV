using UnityEngine;

public class Tip : MonoBehaviour
{
    [SerializeField] private string message;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.KillTip();
            UIManager.Instance.ShowTip(message);
            AudioManager.Instance.PlayTip();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.HideTip();
        }
    }
}
