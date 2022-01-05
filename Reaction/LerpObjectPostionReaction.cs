using UnityEngine;

public class LerpObjectPostionReaction : Reaction
{
    [Header("Lerp position, gameObject can't be static")]
    [SerializeField] private Transform targetTransform = null;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float animationSpeed = 1f;

    public override void TriggerReaction()
    {
        if(targetTransform == null)
            return;
        
        StopAllCoroutines();
        StartCoroutine(TransformLerp.LerpPos(targetTransform, targetPosition, animationSpeed));
    }
}
