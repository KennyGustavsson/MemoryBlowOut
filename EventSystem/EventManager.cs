using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Centralized place to hold all the events for the game.
/// </summary>
public static class EventManager
{ 
    /// <summary>
    /// Called when you complete a puzzle.
    /// </summary>
    public static Action<int> onPuzzleSolved;

    /// <summary>
    /// Called to interact with an <see cref="IInteractable"/>.
    /// </summary>
    public static Action<IInteractable> onMouseClickObject;
    
    /// <summary>
    /// Called to move to a position.
    /// </summary>
    public static Action<Vector3> onMouseClickWalkable;
    
    /// <summary>
    /// Called when you use an item on a receiver.
    /// </summary>
    public static Action<IItemReceiver, InventoryItem> onUseItem;

    /// <summary>
    /// Called to show a <see cref="DialogGroup"/>.
    /// </summary>
    public static Action<DialogGroup> onDisplayDialog;
    
    /// <summary>
    /// Called to stop all running dialogs.
    /// </summary>
    public static Action onStopAllDialogs;
    
    /// <summary>
    /// Called to zoom in to a specific point and rotation.
    /// </summary>
    public static Action<Vector3, Quaternion, float> onCameraZoomIn;
    
    /// <summary>
    /// Called to reset the camera to the normal view.
    /// </summary>
    public static Action onCameraZoomOut;

    public static Action onTimeOutEvent;

    
    /// <summary>
    /// Called to update the time of all things that need to know the time.
    /// </summary>
    public static Action<int> onTimeUpdate;
    /// <summary>
    /// Called when the timer start.
    /// </summary>
    public static Action onStartTimer;

    /// <summary>
    /// Called when you pick up an item.
    /// </summary>
    public static Action<ItemPickup> onItemPickup;

    /// <summary>
    /// Called when you pause the game.
    /// </summary>
    public static Action<bool> onPause;

    /// <summary>
    /// Called when the player dies.
    /// </summary>
    public static Action onPlayerDeath;
    
    /// <summary>
    /// Called when you unlock a clue.
    /// </summary>
    public static Action<int> onUnlockClue;
    
    /// <summary>
    /// Called when you unlock a hint.
    /// </summary>
    public static Action onUnlockHint;

    /// <summary>
    /// Called when you complete a puzzle.
    /// </summary>
    public static Action<GameObject> onPuzzleComplete;

    public static void PuzzleSolved(int puzzleID)
    {
        onPuzzleSolved?.Invoke(puzzleID);
    }
        
    public static void OnMouseClickObject(IInteractable interaction)
    {
        onMouseClickObject?.Invoke(interaction);
    }
    public static void OnMouseClickWalkable(Vector3 position)
    {
        onMouseClickWalkable?.Invoke(position);
    }

    public static void OnUseItem(IItemReceiver receiver, InventoryItem item)
    {
        onUseItem?.Invoke(receiver, item);
    }
    
    public static void DisplayDialog(DialogGroup dialogGroupToDisplay)
    {
        onDisplayDialog?.Invoke(dialogGroupToDisplay);
    }

    public static void StopAllDialogs()
    {
        onStopAllDialogs?.Invoke();
    }

    public static void OnCameraZoomIn(Vector3 position, Quaternion rotation, float speed)
    {
        onCameraZoomIn?.Invoke(position, rotation, speed);
    }

    public static void PlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }

    public static void OnCameraZoomOut()
    {
        onCameraZoomOut?.Invoke();
    }

    public static void TimeOutEvent()
    {
        onTimeOutEvent?.Invoke();
    }

    public static void TimeUpdate(int time)
    {
        onTimeUpdate?.Invoke(time);
    }
    
    public static void OnItemPickup(ItemPickup itemPickup)
    {
        onItemPickup?.Invoke(itemPickup);
    }

    public static void OnPause(bool isPaused)
    {
        onPause?.Invoke(isPaused);
    }

    public static void OnStartTimer()
    {
        onStartTimer?.Invoke();
    }

    public static void OnUnlockClue(int clueID)
    {
        onUnlockClue?.Invoke(clueID);
    }

    public static void OnUnlockHint()
    {
        onUnlockHint?.Invoke();
    }

    public static void OnPuzzleComplete(GameObject obj)
    {
        onPuzzleComplete?.Invoke(obj);
    }
}
