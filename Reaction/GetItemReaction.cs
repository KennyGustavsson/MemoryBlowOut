using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemReaction : Reaction
{
    [SerializeField] private ItemPickup item  = default;

    public override void TriggerReaction()
    {
        EventManager.onItemPickup(item);
    }
}
