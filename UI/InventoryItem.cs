//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, 
    IBeginDragHandler, IEndDragHandler, IDragHandler, 
    IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public InventorySlot slot;

    public ItemPickup.ItemIdentifier itemID;
    public AK.Wwise.Event uiSoundEvent = null;

    public string itemName;
    public string itemDescription = "";

    private RectTransform rectTransform;
    public Canvas canvas;
    private CanvasGroup canvasGroup;

    private Transform inventoryTransform;
    private InventoryUI inventory;

    private DisplayToolTip toolTipDisplay;

    private InventorySlot tempSlot;
    
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        slot = GetComponentInParent<InventorySlot>();

        inventory = GetComponentInParent<InventoryUI>();
        inventoryTransform = GetComponentInParent<InventoryUI>().transform;
        toolTipDisplay = inventory.toolTipDisplay;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        slot.holdingItem = false;
        slot.gameObject.name = "empty";
        slot.draggingContent = true;
        tempSlot = slot;
        rectTransform.SetParent(inventoryTransform);

        if (uiSoundEvent != null)
            uiSoundEvent.Post(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            InteractableItemReceiver receiver = hit.collider.GetComponent<InteractableItemReceiver>();
            if (receiver != null)
            {
                EventManager.OnUseItem(receiver, this);
            }
        }
        tempSlot.draggingContent = false;
        rectTransform.SetParent(slot.transform);
        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        canvasGroup.blocksRaycasts = true;
        slot.holdingItem = true;
        slot.gameObject.name = "holding";
    }

    public void Delete()
    {
        //inventory.inventoryItems.Remove(inventory.inventoryItems.Find((item => item.id == itemID)));
        
        slot.holdingItem = false;
        slot.gameObject.name = "empty";
        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.ChangeHover(MouseCursor.HoverState.ItemInventoryHover);
        if (!toolTipDisplay)
        {
            Debug.LogWarning("No Tooltip UI");
            return;
        }
        toolTipDisplay.Display(itemName, itemDescription, rectTransform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipDisplay.Hide();
    }
}
