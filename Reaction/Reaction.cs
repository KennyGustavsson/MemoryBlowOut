using UnityEngine;

/// <summary>
/// The Abstract Reaction all other reactions inherits from so that we dont have to know what reaction type we want to call
/// </summary>
public abstract class Reaction : MonoBehaviour
{
    /// <summary>
    /// Triggers the reaction. The <see cref="TriggerReaction"/> function should contain the logic for the reaction.
    /// </summary>
    public abstract void TriggerReaction();
}
