using UnityEngine;

public class InteractableSetActiveReaction : Reaction
{
    [SerializeField] private InteractableBase interactable  = default;
    [SerializeField] private bool activeState  = default;
    public override void TriggerReaction()
    {
        interactable.isActive = activeState;
    }
}
