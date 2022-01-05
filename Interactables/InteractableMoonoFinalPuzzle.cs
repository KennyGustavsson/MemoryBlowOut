using UnityEngine;

/// <summary>
/// This is the interactable that triggers the <see cref="FinalPuzzleController"/> which is the drag and drop puzzle.
/// </summary>
public class InteractableMoonoFinalPuzzle : InteractableBase, IInteractable
{

    [SerializeField] private FinalPuzzleController finalPuzzleController = null;
    
    /// <summary>
    /// Function to toggle this script on and off. Might not be used but I dare not remove it.
    /// </summary>
    /// <param name="state"></param>
    public void SetEnabled(bool state)
    {
        isActive = state;
    }
    
    public void OnInteract()
    {
        if (isActive)
        {
            finalPuzzleController.ActivatePuzzle(gameObject);
        }
    }
}
