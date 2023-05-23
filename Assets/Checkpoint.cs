using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("checkpoint updated");
            ProgressionManager.Instance.Progression.SetLastCheckpoint(transform.position);
        }
    }
}
