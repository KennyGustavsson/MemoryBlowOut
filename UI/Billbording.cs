using UnityEngine;

public class Billbording : MonoBehaviour
{
	private Transform myTransform;
	private Transform cameraTransform;

	private void Awake()
	{
		myTransform = transform;
		cameraTransform = Camera.main.transform;
	}

	private void Update()
	{
		var rot = Quaternion.LookRotation(cameraTransform.position - myTransform.position);
		Vector3 rotEulerAngles = rot.eulerAngles;
		Quaternion finalRot = Quaternion.Euler(0, rotEulerAngles.y - 180, 0);

		myTransform.rotation = finalRot;

	}
}
