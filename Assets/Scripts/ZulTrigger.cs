using UnityEngine;

public class ZulTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<ZulAI>().SetIsTriggered(true);
            AudioManager.Instance.ChangeMusic(AudioManager.Instance.ZulBattleMusicClip);
            Destroy(gameObject);
        }
    }
}
