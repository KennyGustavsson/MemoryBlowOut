using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAllDialogsReaction : Reaction
{
    public override void TriggerReaction()
    {
        EventManager.StopAllDialogs();
    }
}
