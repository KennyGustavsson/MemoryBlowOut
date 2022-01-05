using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private Reaction reactionContainerEnter = null;
    [SerializeField] private Reaction reactionContainerExit = null;
    
    [SerializeField] private bool triggerOnce  = default;

    private bool hasEnteredTriggered;
    private bool hasExitedTriggered;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && (!triggerOnce || !hasEnteredTriggered))
        {
            hasEnteredTriggered = true;
            if (reactionContainerEnter)
            {
                reactionContainerEnter.TriggerReaction();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && (!triggerOnce || !hasExitedTriggered))
        {
            hasExitedTriggered = true;
            if (reactionContainerExit)
            {
                reactionContainerExit.TriggerReaction();
            }
        }
    }
}
