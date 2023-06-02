using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetCollected();
            Destroy(gameObject);
        }
    }

    public void GetCollected()
    {
        GameObject.Find("Scene Loader").GetComponent<SceneLoader>().SetIsKeyCollected(true);
        AudioManager.Instance.PlayPickupPowerup();
        UIManager.Instance.SetKeyDisplay(true);
    }
}
