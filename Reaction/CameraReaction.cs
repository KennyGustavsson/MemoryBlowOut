using UnityEngine;

public class CameraReaction : Reaction
{
	[Tooltip("If not specified it will be the transform of the gameObject the script sits on")]
	[SerializeField] private Transform transformToZoomTo;

	[SerializeField] private float speed = 3;
	private enum CameraZoom
	{
		ZoomIn,
		ZoomOut
	}

	[SerializeField] private CameraZoom cameraZoomType  = default;

	public override void TriggerReaction()
	{
		transformToZoomTo = transformToZoomTo ? transformToZoomTo : transform;
		
		switch ((int)cameraZoomType){
			case 0:
				EventManager.OnCameraZoomIn(transformToZoomTo.position, transformToZoomTo.rotation, speed);
				break;
			case 1:
				EventManager.OnCameraZoomOut();
				break;
		}
	}
}
