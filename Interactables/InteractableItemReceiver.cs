using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Used for all Interactables that accept an item/items and in return give you a new item or triggers a reaction.
/// </summary>
public class InteractableItemReceiver : InteractableBase, IInteractable, IItemReceiver
{
    /// <summary>
    /// Class that contains the correct items that the receiver accepts and linked return item and <see cref="Reaction"/>
    /// </summary>
    [System.Serializable]
    public class itemAnswers
    {
        public List<ItemPickup.ItemIdentifier> itemsNeeded;
        [HideInInspector] public bool[] itemsReceived;
        public ItemPickup[] returnItem;
        public Reaction reaction;
    }

    /// <summary>
    /// A struct that contains an <see cref="ItemPickup.ItemIdentifier"/> and a linked <see cref="Reaction"/>.
    /// </summary>
    [System.Serializable]
    public struct ItemUseReaction
    {
        public ItemPickup.ItemIdentifier itemID;
        public Reaction reaction;
    }

    /// <summary>
    /// Bool that decide if the last item before you complete a answer should also trigger a feedback
    /// <see cref="Reaction"/>.
    /// </summary>
    [Tooltip("Should it play item feedbacks when you complete that puzzle as well.")]
    [SerializeField] private bool playFeedbackOnComplete  = default;

    /// <summary>
    /// The different answers to this item receiver.
    /// </summary>
    [Space]
    public List<itemAnswers> answers  = default;
    /// <summary>
    /// The different feedback <see cref="Reaction"/>s that you can get when you put an item into this receiver.
    /// </summary>
    public List<ItemUseReaction> itemUseReactions  = default;

    /// <summary>
    /// The hint <see cref="Reaction"/> you get when you interact with this receiver without holding an item.
    /// </summary>
    [Space]
    [SerializeField] private Reaction hintReaction  = default;
    
    private List<ItemPickup.ItemIdentifier> receivedItems = new List<ItemPickup.ItemIdentifier>();
    private List<int> indexSkips = new List<int>();



    private void Awake()
    {
        for (int i = 0; i < answers.Count; i++)
        {
            answers[i].itemsReceived = new bool[answers[i].itemsNeeded.Count];
        }
    }
    
    /// <summary>
    /// Trigger hint reaction on normal interact if there is one.
    /// </summary>
    public void OnInteract()
    {
        if(hintReaction != null)
            hintReaction.TriggerReaction();
    }

    /// <summary>
    /// the code that is played when you try to insert an item in this item receiver.
    /// </summary>
    /// <param name="item"></param> The item that the player is trying to input.
    public void ReceiveItem(InventoryItem item)
    {
        //Goes through the items and compares them to the items needed in the various answers for the item receiver and
        //deletes the item from the player if it is part of an answer and then adds it to the received items list.
        int index = -1;
        for (int i = 0; i < answers.Count; i++)
        {
            for (int j = 0; j < answers[i].itemsNeeded.Count; j++)
            {
                if (answers[i].itemsNeeded[j] == item.itemID && !indexSkips.Contains(j))
                {
                    Debug.Log("I just ate up item " + item);

                    index = j;
                    answers[i].itemsReceived[j] = true;
                    receivedItems.Add(item.itemID);
                    if (item)
                        item.Delete();
                }
            }
        }

        if (index != -1)
        {
            indexSkips.Add(index);
        }
        
        for (int k = 0; k < indexSkips.Count; k++)
        {
            Debug.Log("I am Holding " + indexSkips[k] + " in slot " + k);
        }

        //checks if there is any answers that have all the items it needs.
        bool completeAnswer = false;
        foreach (var answer in answers)
        {

            completeAnswer = Array.TrueForAll(answer.itemsReceived, (bool received) => received);
            if (completeAnswer)
            {
                Debug.Log("The reaction should be called");
                if (answer.reaction != null)
                {
                    answer.reaction.TriggerReaction();
                }
                if (answer.returnItem != null)
                {
                    foreach (var returnItem in answer.returnItem){
                        if(returnItem)
                            EventManager.OnItemPickup(returnItem);   
                    }
                }
                break;
            }
        }

        //Resets the item receiver so that you can do the same answer more than one time.
        if (completeAnswer)
        {
            //reset
            foreach (var answer in answers)
            {
                for (int i = 0; i < answer.itemsReceived.Length; i++)
                {
                    answer.itemsReceived[i] = false;
                }
            }

            if (playFeedbackOnComplete)
            {
                ItemFeedback(item);
            }

            indexSkips.Clear();
        }
        else
        {
            ItemFeedback(item);
        }
    }

    /// <summary>
    /// Triggers feedback according to the received item.
    /// </summary>
    /// <param name="item"></param>
    private void ItemFeedback(InventoryItem item)
    {
        var itemFeedback = itemUseReactions.Find(
            (ItemUseReaction useReaction) => item.itemID == useReaction.itemID);
        itemFeedback.reaction?.TriggerReaction();
        hintReaction = itemFeedback.reaction;

    }
}
