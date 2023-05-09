using UnityEngine;

public class SpellContainer : MonoBehaviour
{
    [SerializeField] private GameObject childSpell;

    private void Update()
    {
        if (childSpell == null)
        {
            Destroy(gameObject, 1f);
        }
    }
}
