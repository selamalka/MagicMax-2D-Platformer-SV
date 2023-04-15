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
        button = GetComponent<Button>();
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
        draggedIcon = new GameObject("Dragged Icon");        
        Image iconImage = draggedIcon.AddComponent<Image>();
        iconImage.sprite = GetComponent<Image>().sprite;
        draggedIcon.transform.SetParent(GameObject.FindWithTag("Canvas").transform, false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggedIcon.transform.position = Input.mousePosition;
        Debug.Log("Pointer entered: " + eventData.pointerEnter.name);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SpellSlot spellSlot = eventData.pointerEnter.GetComponent<SpellSlot>();
        if (spellSlot != null)
        {
            transform.SetParent(spellSlot.transform, false);
            transform.position = spellSlot.transform.position;        
        }
        else
        {
            print("spellSlot is null");
            Destroy(draggedIcon);
        }
    }
}
