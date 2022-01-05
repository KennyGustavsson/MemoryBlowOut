using UnityEngine;

/// <summary>
/// This Interactable is no longer used anywhere so it should be removed.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractableMonoToggle : InteractableBase, IInteractable
{

    [SerializeField] private InteractableMonoVault vault = default;
    [SerializeField] private ReactionContainer onInteractReactions = default;
    [SerializeField] Material unInteractable = default;

    public void OnInteract()
    {
        if (isActive)
        {
            if (onInteractReactions)
            {
                onInteractReactions.TriggerReactions();
            }
            vault.decrement();
            GetComponent<MeshRenderer>().material = unInteractable;
        }
    }


}
