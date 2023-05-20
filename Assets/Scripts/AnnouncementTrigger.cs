using System.Threading.Tasks;
using UnityEngine;

public class AnnouncementTrigger : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] private float duration;
    [SerializeField] private string followMessage;
    [SerializeField] private float followDuration;
    private bool hasBeenTriggered;

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!hasBeenTriggered)
            {
                UIManager.Instance.Announcement(message, duration);

                await Task.Delay(Mathf.FloorToInt(duration) * 1000);

                if (followMessage != "")
                {
                    UIManager.Instance.Announcement(followMessage, followDuration);
                }
            }
            hasBeenTriggered = true;
        }
    }
}
