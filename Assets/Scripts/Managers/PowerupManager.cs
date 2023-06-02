using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] private List<GameObject> powerups = new List<GameObject>();
    [field: SerializeField] public int DropChance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void DropRandomPowerup(Transform transform, int dropChance)
    {
        var randomRoll = Random.Range(0, 100);

        if (randomRoll <= dropChance)
        {
            Instantiate(GetRandomPowerup(), transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            AudioManager.Instance.PlayPowerupDrop();
        }
    }

    public GameObject GetRandomPowerup()
    {
        var randomInt = Random.Range(0, powerups.Count);
        return powerups[randomInt];
    }
}
