using System.Collections;
using UnityEngine;

/// <summary>
/// The interactable that gives you an item when you interact with it.
/// </summary>
public class InteractablePickup : InteractableBase, IInteractable
{

    /// <summary>
    /// The item you get when you interact with this Interactable.
    /// </summary>
    [SerializeField] private ItemPickup item = default;
    /// <summary>
    /// The reaction that is triggered when you pick up this item.
    /// </summary>
    [SerializeField] private Reaction reaction;
    
    public void OnInteract()
    {
        if(!isActive) return;

        if (reaction != null)
        {
            reaction.TriggerReaction();
        }

        StartCoroutine(PickUp());
    }


    /// <summary>
    /// Function that plays an animation where the item goes into the players inventory. it is pretty hard coded but we
    /// didn't have any time to do it the proper way.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PickUp()
    {
        Transform pickUpTransform = transform;
        Transform playerTransform = GameManager.Instance.playerObj.transform;

        Vector3 originalScale = pickUpTransform.localScale;
        Vector3 targetScale = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 originalPosition = pickUpTransform.position;
        Vector3 targetOffset = new Vector3(0, 1, 0.1f);
        
        float t = 0;
        
        while (t<1)
        {
            float x = EaseInCubic(t);
            
            //Decreases the size of the object as it approaches the player.
            pickUpTransform.localScale = Vector3.Lerp(originalScale, targetScale, x);
            
            //moves the object in a curve towards the player.
            pickUpTransform.position = TranslationCurveLerp.LerpCurve(originalPosition,
                playerTransform.TransformPoint(targetOffset),
                Vector3.up + Vector3.right / 2, 1.5f, x);
            
            t += Time.deltaTime;

            yield return null;
        }
        
        //triggers an event that adds the item to the player inventory
        EventManager.OnItemPickup(item);
        Destroy(gameObject);
    }
    
    
    /// <summary>
    /// An ease function... Do i need to explain this?
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private static float EaseInCubic(float x)
    {
        return x * x * x;
    }

}
