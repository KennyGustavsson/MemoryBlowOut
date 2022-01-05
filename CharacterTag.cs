using System;
using UnityEngine;

public class CharacterTag : MonoBehaviour
{
    [SerializeField] private Character character = default;    
    
    private void Awake()
    {
        if (characters[(int) character])
        {
            Debug.LogError("There is multiple characters of the same type " + characters[(int)character]);
        }
        else
        {
            characters[(int)character] = this;
        }

    }

    public static CharacterTag[] characters = new CharacterTag[Enum.GetNames(typeof(Character)).Length];

    public Vector2 dialogOffset = Vector2.up * 150;
    public Color dialogColor = Color.black;
    public Color dialogOutlineColor = Color.white;
    public float dialogOutlineThickness = 1;

    public enum Character
    {
        Player,
        Boss,
        Guard,
        Employee
    }
}
