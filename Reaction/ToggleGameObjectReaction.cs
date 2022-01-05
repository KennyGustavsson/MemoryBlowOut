using UnityEngine;

public class ToggleGameObjectReaction : Reaction
{
    [SerializeField] private GameObject gameObjectToToggle  = null;
    [SerializeField] private bool toggleState = false;
    
    public override void TriggerReaction()
    {
        gameObjectToToggle.SetActive(toggleState);
    }
}
