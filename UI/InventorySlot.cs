using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class InventorySlot : MonoBehaviour, IDropHandler
{
    public bool holdingItem;
    //public InventoryItem heldItem;

    private RectTransform rectTransform;
    private InventoryUI inventory;

    public bool draggingContent = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        holdingItem = GetComponentInChildren<InventoryItem>() != null;
        inventory = GetComponentInParent<InventoryUI>();
    }


    public void OnDrop(PointerEventData eventData)
    {

        //Catch item
        if (eventData.pointerDrag != null)
        {
            InventoryItem dropItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            RectTransform dropItemTransform = dropItem.GetComponent<RectTransform>();
            
            if (holdingItem)
            {
                //move occupying item to other slot
                

                InventorySlot otherSlot = dropItem.slot;
                RectTransform otherSlotTransform = otherSlot.rectTransform;

                InventoryItem heldItem = GetComponentInChildren<InventoryItem>();

                heldItem.transform.SetParent(otherSlotTransform);
                heldItem.transform.position = otherSlotTransform.position;
                
                heldItem.slot = otherSlot;

                otherSlot.holdingItem = true;
                otherSlot.gameObject.name = "holding";
            }            

            dropItemTransform.SetParent(rectTransform);
            dropItemTransform.position = rectTransform.position;
            dropItem.slot = this;
        }

    }
}

