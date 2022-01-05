using UnityEngine;

public class AudioStateReaction : Reaction
{
    [SerializeField] private AK.Wwise.State audioState = default;
    
    public override void TriggerReaction()
    {
        audioState.SetValue();
    }
}
