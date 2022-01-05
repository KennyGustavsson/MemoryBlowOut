using UnityEngine;
using UnityEngine.Events;

public class UnityEventReaction : Reaction
{
    [SerializeField] private UnityEvent onTriggerReaction = default;
    public override void TriggerReaction()
    {
        onTriggerReaction?.Invoke();
    }
}
