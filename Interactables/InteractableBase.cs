using UnityEngine;

/// <summary>
/// Abstract class that all interactables inherits from.
/// </summary>
public abstract class InteractableBase : MonoBehaviour
{
    /// <summary>
    /// Is the interactable open to accept interactions.
    /// </summary>
    public bool isActive = true;
    /// <summary>
    /// The position that the player character walks to to interact.
    /// </summary>
    public Vector3 waypointPosition = Vector3.zero;
    /// <summary>
    /// If the player should also rotate to a custom rotation when interacting with the object. If false the player will
    /// rotate so that it is facing to the interactable object pivot.
    /// </summary>
    public bool useCustomRotation = false;
    /// <summary>
    /// The Rotation to rotate to if <see cref="useCustomRotation"/> is true.
    /// </summary>
    public Vector3 waypointRotation = Vector3.zero;
}
