using System.Threading.Tasks;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("checkpoint updated");
            ProgressionManager.Instance.Progression.SetLastCheckpoint(transform.position);
            UIManager.Instance.ShowTip("Checkpoint Saved");
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
