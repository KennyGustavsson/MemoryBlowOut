using System.Collections;
using UnityEngine;

public class ReactionContainer : Reaction
{
    [SerializeField] private Reaction[] reactions = default;
    
    [Header("Delay")]
    [SerializeField] private float reactionDelay = 0;

    public void TriggerReactions()
    {
        if(reactions.Length == 0) return;
        foreach (Reaction reaction in reactions)
        {
            if(!reaction) return;
            if (reaction == this)
            {
                Debug.LogError("Ops this should not happen call Filip He did something awful");
                continue;
            }
            reaction.TriggerReaction();
        }
    }

    public override void TriggerReaction()
    {
        
        if (reactionDelay == 0)
        {
            TriggerReactions();
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(DelayedReaction());
        }
    }

    private IEnumerator DelayedReaction()
    {
        yield return new WaitForSeconds(reactionDelay);
        TriggerReactions();
    }
}
