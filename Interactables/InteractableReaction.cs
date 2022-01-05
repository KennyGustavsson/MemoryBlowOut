using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// An interactable tha only triggers a <see cref="Reaction"/>.
/// </summary>
public class InteractableReaction : InteractableBase, IInteractable
{
    [SerializeField] private Reaction reaction = default;
    

    public void OnInteract()
    {
        if (reaction != null)
            reaction.TriggerReaction();
    }
}
