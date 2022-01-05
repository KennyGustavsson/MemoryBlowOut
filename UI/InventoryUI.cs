using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    // [SerializeField] private Button LeftButton;
    // [SerializeField] private Button RightButton;
    // [SerializeField] private Button TopButton;

    public DisplayToolTip toolTipDisplay  = default;

    private List<InventorySlot> itemSlots;
    private int currentIndex = 0;

    [SerializeField] private Transform[] slotPositions  = default;
    [SerializeField] private InventoryItem itemBasePrefab  = default;
    [SerializeField] private InventorySlot slotPrefab  = default;

    [Header("Hide Positions")]
    [SerializeField] private RectTransform hidableInventoryObj  = default;
    [SerializeField] private float hideYPos = -100;
    private bool isHidden = false;
    private bool isInAnimation = false;

    private void Awake()
    {
        itemSlots = new List<InventorySlot>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<InventorySlot>();
            if (child != null)
                itemSlots.Add(child);
        }
    }

    private void OnEnable()
    {
        EventManager.onItemPickup += AddItem;
    }

    private void OnDisable()
    {
        EventManager.onItemPickup -= AddItem;
    }
    
    private void AddItem(ItemPickup item)
    {
        var emptySlot = itemSlots.Find((InventorySlot slot) => (!slot.holdingItem && !slot.draggingContent));
        bool wasinstantiated = false;
        
        if (emptySlot == null)
        {
            emptySlot = Instantiate(slotPrefab, transform);
            itemSlots.Add(emptySlot);
            wasinstantiated = true;
        }

        emptySlot.holdingItem = true;
        InventoryItem newItem = Instantiate(itemBasePrefab, emptySlot.transform);
        newItem.itemID = item.id;
        newItem.itemDescription = item.itemDescription;
        newItem.itemName = item.itemName;
        newItem.GetComponent<Image>().sprite = item.UIImage;
        newItem.uiSoundEvent = item.uiSoundEvent;

        newItem.GetComponent<ButtonFlashAnimator>().StartAnimation();

        if (wasinstantiated)
        {
            emptySlot.gameObject.SetActive(false);
        }
    }

    public InventorySlot GetEmptySlot()
    {
        var emptySlot = itemSlots.Find((InventorySlot slot) => !slot.holdingItem);

        if (emptySlot == null)
        {
            emptySlot = Instantiate(slotPrefab, transform);
            itemSlots.Add(emptySlot);
            emptySlot.gameObject.SetActive(false);
        }

        return emptySlot;
    }

    public void OnClickLeft()
    {
        if(itemSlots.Count <= slotPositions.Length)
            return;

        itemSlots[CircleIndex(slotPositions.Length-1)].gameObject.SetActive(false);

        currentIndex = CircleDecrement(currentIndex);

        itemSlots[CircleIndex(0)].gameObject.SetActive(true);

        UpdateSlotPositions();
    }

    public void OnClickRight()
    {
        if(itemSlots.Count <= slotPositions.Length)
            return;

        itemSlots[CircleIndex(0)].gameObject.SetActive(false);

        currentIndex = CircleIncrement(currentIndex);

        itemSlots[CircleIndex(slotPositions.Length - 1)].gameObject.SetActive(true);

        UpdateSlotPositions();
    }

    private int CircleDecrement(int x)
    {
        x--;
        return nfmod(x, itemSlots.Count);
    }

    private int CircleIncrement(int x)
    {
        x++;
        return nfmod(x, itemSlots.Count);
    }

    private int CircleIndex(int x)
    {
        return nfmod(currentIndex + x, itemSlots.Count);
    }

    int nfmod(int a, int b)
    {
        return a - b * (int)Math.Floor((double)a / (double)b);
    }
    
    private void UpdateSlotPositions()
    {
        for (int i = 0; i < slotPositions.Length; i++)
        {
            itemSlots[CircleIndex(i)].transform.position = slotPositions[i].position;
        }
    }
    
    public void OnClickTop()
    {
        if(isInAnimation)
            return;
        
        StopAllCoroutines();

        if (isHidden)
        {
            isHidden = false;
            isInAnimation = true;
            StartCoroutine(EaseUp(hidableInventoryObj, 1f));
        }
        else
        {
            isHidden = true;
            isInAnimation = true;
            StartCoroutine(EaseDown(hidableInventoryObj, 1f));
        }
    }
    
    private IEnumerator EaseDown(RectTransform transform, float speed)
    {
        float time = 0;
        while (time <= 1)
        {
            transform.anchoredPosition = new Vector2(0, easeInOutBack(time) * hideYPos);

            yield return new WaitForEndOfFrame();
            time += Time.deltaTime * speed;
        }
    
        transform.anchoredPosition = new Vector2(0, hideYPos);
        isInAnimation = false;
    }
    
    private IEnumerator EaseUp(RectTransform transform, float speed)
    {
        float time = 1;
        while (time >= 0)
        {
            transform.anchoredPosition = new Vector2(0, easeInOutBack(time) * hideYPos);

            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime * speed;
        }
    
        transform.anchoredPosition = new Vector2(0, 0);
        isInAnimation = false;
    }
    
    private float easeInOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return x < 0.5
            ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
            : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
    }
}
