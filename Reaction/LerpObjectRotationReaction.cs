using UnityEngine;

public class LerpObjectRotationReaction : Reaction
{
	[Header("Lerp rotation, gameObject can't be static")]
	[SerializeField] private Transform targetTransform = null;
	[SerializeField] private Vector3 targetRotation = Vector3.zero;
	[SerializeField] private float animationSpeed = 1f;

	public override void TriggerReaction()
	{
		if(targetTransform == null)
			return;
        
		Quaternion rotation = Quaternion.Euler(targetRotation);
		
		StopAllCoroutines();
		StartCoroutine(TransformLerp.LerpRot(targetTransform, rotation, animationSpeed));
	}
}
