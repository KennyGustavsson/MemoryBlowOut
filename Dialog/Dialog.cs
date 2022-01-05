using UnityEngine;

/// <summary>
/// Contains all the information needed to display a dialog.
/// </summary>
[CreateAssetMenu(fileName = "Dialog", menuName = "Dialog/Dialog")]
public class Dialog : ScriptableObject
{
    public CharacterTag.Character dialogCharacter;    
    public float dialogTime;
    [@TextAreaAttribute(10, 20)]
    public string dialogText;
}