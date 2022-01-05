public class PlayerDeathReaction : Reaction
{
    public override void TriggerReaction()
    {
        EventManager.PlayerDeath();
    }
}
