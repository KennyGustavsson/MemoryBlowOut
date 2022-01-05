using UnityEngine;

/// <summary>
/// The Interactable that Starts the <see cref="PuzzleRotatable"/> which is the rotatable tile puzzle.
/// </summary>
public class InteractableRotateable : InteractableBase, IInteractable
{
    [SerializeField] private PuzzleRotatable puzzleRotatable = default;

    public void OnInteract()
    {
        if (isActive)
        {
            puzzleRotatable.ActivatePuzzle();
        }
    }

    public void EnableInteractable()
    {
        isActive = false;
    }
    
    public void DisableInteractable()
    {
        isActive = true;
    }
}
