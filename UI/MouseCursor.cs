using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Texture2D noActionCursorCursorTexture = default;
    [SerializeField] private Texture2D interactableCursorTexture = default;
    [SerializeField] private Texture2D moveCursorTexture = default;
    [SerializeField] private Texture2D inventoryCursorTexture = default;
    [SerializeField] private Texture2D itemPlaceableCursor = default;
    [SerializeField] private Texture2D itemGrabbedCursor = default;
    [SerializeField] private Vector2 hotSpot = default;

    [Header("Reaction")]
    [SerializeField] private Reaction hoverInteractableCursor = default;
    [SerializeField] private Reaction hoverInventoryItemCursor = default;
    [SerializeField] private Reaction hoverGrabbableCursor = default;


    private HoverState hoverState = default;

    private CursorMode cursorMode = CursorMode.Auto;
    
    public enum HoverState
    {
        Normal,
        Walkable,
        Interactable,
        HoverItemInteractable,
        ItemPlaceable,
        ItemGrabbed,
        UIButton,
        Tile,
        ItemInventoryHover,
        HoverCharacter
    }

    public enum CursorAppearance
    {
        MoveCursor,
        InteractCursor,
        NormalCursor,
        ItemInventoryCursor,
        ItemPlaceableCursor,
        ItemGrabbed
    }

    public void SetHover(HoverState hover)
    {
        if(hoverState == hover) return;

        hoverState = hover;
        switch (hover)
        {
            case HoverState.Walkable:
                ChangeCursor(CursorAppearance.MoveCursor);
                break;
            case HoverState.Interactable:
                if (hoverInteractableCursor) hoverInteractableCursor.TriggerReaction();
                ChangeCursor(CursorAppearance.InteractCursor);
                break;
            case HoverState.HoverItemInteractable:
                break;
            case HoverState.ItemPlaceable:
                ChangeCursor(CursorAppearance.ItemPlaceableCursor);
                if (hoverGrabbableCursor) hoverGrabbableCursor.TriggerReaction();

                break;
            case HoverState.UIButton:
                break;
            case HoverState.Tile:
                break;
            case HoverState.ItemInventoryHover:
                ChangeCursor(CursorAppearance.ItemInventoryCursor);
                if (hoverInventoryItemCursor) hoverInventoryItemCursor.TriggerReaction();
                break;
            case HoverState.HoverCharacter:
                break;
            case HoverState.Normal:
                ChangeCursor(CursorAppearance.NormalCursor);
                break;
            case HoverState.ItemGrabbed:
                ChangeCursor(CursorAppearance.ItemGrabbed);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hover), hover, null);
        }
    }
    

    private void Start()
    {
        GameManager.Instance.MouseCursor = this;
        ChangeCursor(CursorAppearance.InteractCursor);
    }

    private void ChangeCursor(CursorAppearance cursor)
    {
        switch (cursor)
        {
            case CursorAppearance.MoveCursor:
                Cursor.SetCursor(moveCursorTexture, hotSpot, cursorMode);
                break;
            case CursorAppearance.InteractCursor:
                Cursor.SetCursor(interactableCursorTexture, hotSpot, cursorMode);
                break;
            case CursorAppearance.NormalCursor:
                Cursor.SetCursor(noActionCursorCursorTexture, hotSpot, cursorMode);
                break;
            case CursorAppearance.ItemInventoryCursor:
                Cursor.SetCursor(inventoryCursorTexture, hotSpot, cursorMode);
                break;
            case CursorAppearance.ItemPlaceableCursor:
                Cursor.SetCursor(itemPlaceableCursor, hotSpot, cursorMode);
                break;
            case CursorAppearance.ItemGrabbed:
                Cursor.SetCursor(itemGrabbedCursor, hotSpot, cursorMode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cursor), cursor, null);
        }
    }

}
