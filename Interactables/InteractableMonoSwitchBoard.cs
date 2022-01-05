using UnityEngine;

/// <summary>
/// The interactable part that starts the <see cref="switchBoard"/> puzzle.
/// </summary>
public class InteractableMonoSwitchBoard : InteractableBase, IInteractable
{
    [SerializeField] private Reaction onInteractReaction  = null;
    [SerializeField] private SwitchBoard switchBoard = null;
    
    public void OnInteract()
    {
        if (!isActive) return;

        switchBoard.isActive = true;
        
        //This is so that we can click on objects inside the collider of this object. it disables raycasts for this
        //object
        gameObject.layer = 2;

        if (onInteractReaction)
        {
            onInteractReaction.TriggerReaction();
        }
    }
}