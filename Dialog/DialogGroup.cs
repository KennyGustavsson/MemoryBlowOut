using UnityEngine;

/// <summary>
/// A <see cref="ScriptableObject"/> to contain an Array of <see cref="Dialog"/>s.
/// </summary>
[CreateAssetMenu(fileName = "DialogName", menuName = "Dialog/DialogGroup")]
public class DialogGroup : ScriptableObject
{
    public Dialog[] dialogs;
}
