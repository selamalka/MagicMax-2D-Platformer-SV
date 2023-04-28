using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISpell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [field: SerializeField] public GameObject SpellPrefab { get; private set; }
    [field: SerializeField] public SpellData SpellData { get; private set; }

    [SerializeField] private Image blockedImage;
    private GameObject draggedIcon;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        SpellData.SetIsSpellUnlocked(false);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(UnlockSpell);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(UnlockSpell);
    }

    private void Start()
    {
        GetComponent<Image>().sprite = SpellData.Sprite;
    }

    private void UnlockSpell()
    {
        if (SpellData.Level == 1)
        {
            blockedImage.color = blockedImage.color.a > 0 ? new Color(0, 0, 0, 0) : blockedImage.color;
            SpellData.SetIsSpellUnlocked(true);
            SpellbookManager.Instance.UnlockedSpells.Add(SpellData);
            PlayerStatsManager.Instance.SetSpellPoints(PlayerStatsManager.Instance.SpellPoints - SpellData.SpellPointCost);
            UIManager.Instance.UpdateSpellPoints();
        }
         // NEED TO MAKE CONDITIONS FOR NEXT LEVEL
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (SpellData.Level == 0) return;
        draggedIcon = new GameObject(SpellData.Name);
        Image iconImage = draggedIcon.AddComponent<Image>();
        iconImage.sprite = GetComponent<Image>().sprite;
        iconImage.raycastTarget = false;
        draggedIcon.transform.SetParent(transform.root, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SpellData.Level == 0) return;
        draggedIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (SpellData.Level == 0) return;
        SpellSlot spellSlot = eventData.pointerEnter.GetComponent<SpellSlot>();

        if (spellSlot != null)
        {
            draggedIcon.transform.SetParent(spellSlot.transform, false);
            draggedIcon.transform.position = spellSlot.transform.position;
            spellSlot.SetCurrentSpell(this);
        }
        else
        {
            Destroy(draggedIcon);
        }
    }
}