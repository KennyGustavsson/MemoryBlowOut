using System.Collections;
using UnityEngine;

public static class TransformLerp
{
	public static IEnumerator LerpPos(Transform transform, Vector3 targetPos, float speed)
	{
		float time = 0;

		Vector3 localPos = transform.localPosition;
		
		while (transform.localPosition != targetPos || time > 5){
			transform.localPosition = Vector3.Lerp(localPos, targetPos, time * speed);
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
		}

		transform.localPosition = targetPos;
	}

	public static IEnumerator LerpRot(Transform transform, Quaternion targetRot, float speed)
	{
		float time = 0;

		Quaternion localRot = transform.localRotation;
		
		while (transform.localRotation != targetRot || time > 5)
		{
			transform.localRotation = Quaternion.Slerp(localRot, targetRot, time * speed);
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
		}
        
		transform.localRotation = targetRot;
	} 
}
