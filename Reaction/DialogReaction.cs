
public class DialogReaction : Reaction
{
    public DialogGroup dialogGroup;
    public override void TriggerReaction()
    {
        EventManager.onDisplayDialog(dialogGroup);
    }
}
