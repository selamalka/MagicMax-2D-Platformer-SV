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

        SpellData.SetIsUnlocked(false);

        if (ProgressionManager.Instance.Progression.UnlockedSpellsList.Contains(SpellData))
        {
            SpellData.SetIsUnlocked(true);
            blockedImage.color = new Color(0, 0, 0, 0);
        }
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
        if (PlayerStatsManager.Instance.SpellPoints - SpellData.SpellPointCost < 0 || SpellData.IsUnlocked) return;
        if (SpellData.Level == 1)
        {
            blockedImage.color = blockedImage.color.a > 0 ? new Color(0, 0, 0, 0) : blockedImage.color;
            SpellData.SetIsUnlocked(true);
            ProgressionManager.Instance.Progression.UnlockedSpellsList.Add(SpellData);
            PlayerStatsManager.Instance.SetSpellPoints(PlayerStatsManager.Instance.SpellPoints - SpellData.SpellPointCost);
            UIManager.Instance.UpdateSpellPoints();
        }
        else if (GetPreviousSpellTier().IsUnlocked)
        {
            blockedImage.color = blockedImage.color.a > 0 ? new Color(0, 0, 0, 0) : blockedImage.color;
            SpellData.SetIsUnlocked(true);
            ProgressionManager.Instance.Progression.UnlockedSpellsList.Add(SpellData);
            PlayerStatsManager.Instance.SetSpellPoints(PlayerStatsManager.Instance.SpellPoints - SpellData.SpellPointCost);
            UIManager.Instance.UpdateSpellPoints();
        }
    }
    private SpellData GetPreviousSpellTier()
    {
        SpellData spell = SpellManager.Instance.FindSpellByNameAndLevel(SpellData.Name, SpellData.Level - 1);
        return spell;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (SpellData.Level == 0 || !SpellData.IsUnlocked) return;
        draggedIcon = new GameObject(SpellData.Name);
        Image iconImage = draggedIcon.AddComponent<Image>();
        iconImage.sprite = GetComponent<Image>().sprite;
        iconImage.raycastTarget = false;
        draggedIcon.transform.SetParent(transform.root, false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (SpellData.Level == 0 || !SpellData.IsUnlocked) return;
        draggedIcon.transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (SpellData.Level == 0 || !SpellData.IsUnlocked) return;
        SpellSlot spellSlot = eventData.pointerEnter.GetComponent<SpellSlot>();

        if (spellSlot != null)
        {
            draggedIcon.transform.SetParent(spellSlot.transform, false);
            draggedIcon.transform.position = spellSlot.transform.position;
            spellSlot.SetCurrentSpell(this);

            if (GameObject.Find("Spell Slot 1").GetComponent<SpellSlot>().SpellSlotNumber == 1)
            {
                ProgressionManager.Instance.Progression.SetSpellSlot1Spell(this);
            }
            else if (GameObject.Find("Spell Slot 2").GetComponent<SpellSlot>().SpellSlotNumber == 2)
            {
                ProgressionManager.Instance.Progression.SetSpellSlot2Spell(this);
            }
        }
        else
        {
            Destroy(draggedIcon);
        }
    }
}