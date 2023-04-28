using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [SerializeField] private List<GameObject> powerups = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void DropRandomPowerup(Transform transform)
    {
        Instantiate(GetRandomPowerup(), transform.position + new Vector3(0,1,0), Quaternion.identity);
    }

    public GameObject GetRandomPowerup()
    {
        var randomInt = Random.Range(0, powerups.Count);
        return powerups[randomInt];
    }
}