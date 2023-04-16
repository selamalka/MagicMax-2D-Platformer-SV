using UnityEngine;

public class SpellbookManager : MonoBehaviour
{
    public static SpellbookManager Instance;

    [field: Header("Spells")]
    [field: SerializeField] public SpellData MagicShot { get; private set; }

    [field: Header("Spell Slots")]
    [SerializeField] private SpellSlot spellSlot1;
    [SerializeField] private SpellSlot spellSlot2;

    private void Awake()
    {
        Instance = this;
    }
}
