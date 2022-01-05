using UnityEngine;

public class TriggerReactionOnTime : MonoBehaviour
{
    [SerializeField] private Reaction reaction;
    [SerializeField] private int onTimeRemaining;

    private bool wasTriggered = false;

    private void OnEnable()
    {
        EventManager.onTimeUpdate += onTimeUpdate;
    }

    private void OnDisable()
    {
        EventManager.onTimeUpdate -= onTimeUpdate;
    }
    
    private void onTimeUpdate(int time)
    {
        if(wasTriggered || time > onTimeRemaining) return;
        
        if(reaction != null)
            reaction.TriggerReaction();

        wasTriggered = true;
    }
}
