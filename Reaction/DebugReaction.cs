using UnityEngine;

public class DebugReaction : Reaction
{
    [SerializeField] private string debugText = default;
    public override void TriggerReaction()
    {
        Debug.Log(debugText);
    }
}
