using UnityEngine;

public class ToggleShowPlayerReaction : Reaction
{
    [SerializeField] private bool showPlayerState = default;
    public override void TriggerReaction()
    {
        GameManager.Instance.ToggleShowPlayer(showPlayerState);
    }
}
