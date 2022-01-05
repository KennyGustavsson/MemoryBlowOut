using UnityEngine;

/// <summary>
/// This no longer used and should be removed.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractableMonoVault : InteractableBase, IInteractable
{
    [SerializeField] private int counter = 3;

    [SerializeField] private Material interactable = default;

    [SerializeField] private GameObject vaultPuzzle = default;

    [SerializeField] private ReactionContainer reactionContainer = null;

    public void decrement()
    {
        if (counter > 0)
            counter--;

        if (counter <= 0)
        {
            GetComponent<MeshRenderer>().material = interactable;
        }
    }


    public void OnHover()
    {
    }

    public void OnInteract()
    {
        if(!isActive) return;
        if (counter <= 0)
        {
            if(reactionContainer)
                reactionContainer.TriggerReactions();
            
            vaultPuzzle.SetActive(true);
            GameManager.Instance.inputController.allowInput = false;
        }
    }



}
