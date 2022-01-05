using UnityEngine;

/// <summary>
/// Controls all camera movement.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offsetDirection = default;
    [SerializeField] private float followDistance = default;
    [SerializeField] private float cameraFollowSpeed = 1;

    [Header("Target Transform")]
    [SerializeField] private Transform target = null;

    [Header("Animation Speed")]
    [SerializeField] private float cameraZoomOutRotSpeed = 1f;

    private bool followTarget = true;
    
    private Quaternion zoomOutRot;

    private void Awake()
    {
        zoomOutRot = transform.rotation;
    }

    private void OnEnable()
    {
        EventManager.onCameraZoomIn += OnCameraZoomIn;
        EventManager.onCameraZoomOut += OnCameraZoomOut;
    }

    private void OnDisable()
    {
        EventManager.onCameraZoomIn -= OnCameraZoomIn;
        EventManager.onCameraZoomOut -= OnCameraZoomOut;
    }

    private void Update()
    {
        if(followTarget)
            PositionCamera();
    }

    private void OnValidate()
    {
        transform.position = target.position + offsetDirection.normalized * followDistance;
        transform.LookAt(target);
    }

    private void PositionCamera()
    {
        if (!target) return;
        transform.position = Vector3.MoveTowards(transform.position,
            target.position + offsetDirection.normalized * followDistance, cameraFollowSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Animates the camera to a position and rotation.
    /// </summary>
    /// <param name="position"></param> The Position to zoom to.
    /// <param name="rotation"></param> The Rotation to rotate to.
    /// <param name="speed"></param>
    private void OnCameraZoomIn(Vector3 position, Quaternion rotation, float speed)
    {
        StopAllCoroutines();
        followTarget = false;
        StartCoroutine(TransformLerp.LerpPos(transform, position, speed));
        StartCoroutine(TransformLerp.LerpRot(transform, rotation, speed));
    }

    /// <summary>
    /// Resets the camera to the standard rotation and position.
    /// </summary>
    private void OnCameraZoomOut()
    {
        StopAllCoroutines();
        followTarget = true;
        StartCoroutine(TransformLerp.LerpRot(transform, zoomOutRot, cameraZoomOutRotSpeed));
    }
}
