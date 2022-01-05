public class StartTimerReaction : Reaction
{
    public override void TriggerReaction()
    {
        EventManager.OnStartTimer();
    }
}
