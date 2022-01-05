using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpReaction : Reaction
{

    [SerializeField] private Transform pickUpTransform = null;
    [SerializeField] private ItemPickup item = default;
    [SerializeField] private Reaction nextReaction = null;


    public override void TriggerReaction()
    {
        GameManager.Instance.playerStateMachine.animator.Play("Grab");
        StartCoroutine(PickUp());
    }

    private IEnumerator PickUp()
    {
        Transform playerTransform = GameManager.Instance.playerObj.transform;

        Vector3 originalScale = pickUpTransform.localScale;
        Vector3 targetScale = new Vector3(0.2f, 0.2f, 0.2f);
        Vector3 originalPosition = pickUpTransform.position;
        Vector3 targetOffset = new Vector3(0, 1, 0.1f);

        float t = 0;

        while (t < 1)
        {
            float x = EaseInCubic(t);

            pickUpTransform.localScale = Vector3.Lerp(originalScale, targetScale, x);

            t += Time.deltaTime;

            pickUpTransform.position = TranslationCurveLerp.LerpCurve(originalPosition,
                playerTransform.TransformPoint(targetOffset),
                Vector3.up + Vector3.right / 2, 1.5f, x);

            yield return null;
        }

        EventManager.OnItemPickup(item);
        Debug.Log("pickup next");
        nextReaction?.TriggerReaction();
        //Destroy(pickUpTransform.gameObject);
        pickUpTransform.gameObject.SetActive(false);
    }

    private static float EaseOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }

    private static float EaseInCubic(float x)
    {
        return x * x * x;
    }

}
