using UnityEngine;

public class AnnouncementTrigger : MonoBehaviour
{
    [SerializeField] private string message;
    [SerializeField] private float duration;
    private bool hasBeenTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!hasBeenTriggered)
            {
                UIManager.Instance.Announcement(message, duration);
            }
            hasBeenTriggered = true;
        }
    }
}
