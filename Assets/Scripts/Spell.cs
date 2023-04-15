using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Spell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private SpellData spellData;
    [SerializeField] private TextMeshProUGUI levelValue;
    [SerializeField] private Image blockedImage;
    [SerializeField] private int maxLevel;
    [SerializeField] private int level;
    private GameObject draggedIcon;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(AddLevel);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(AddLevel);
    }

    private void Start()
    {
        GetComponent<Image>().sprite = spellData.Sprite;
    }

    private void AddLevel()
    {
        blockedImage.color = blockedImage.color.a > 0 ? new Color(0, 0, 0, 0) : blockedImage.color;

        if (level < maxLevel)
        {
            level++;
            levelValue.text = level.ToString();
        }
        else
        {
            print("spell is at max level");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedIcon = new GameObject(spellData.Name);
        Image iconImage = draggedIcon.AddComponent<Image>();
        iconImage.sprite = GetComponent<Image>().sprite;
        iconImage.raycastTarget = false;
        draggedIcon.transform.SetParent(transform.root, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggedIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SpellSlot spellSlot = eventData.pointerEnter.GetComponent<SpellSlot>();

        if (spellSlot != null)
        {
            draggedIcon.transform.SetParent(spellSlot.transform, false);
            draggedIcon.transform.position = spellSlot.transform.position;
            spellSlot.SetCurrentSpell(spellData);
        }
        else
        {
            Destroy(draggedIcon);
        }
    }
}