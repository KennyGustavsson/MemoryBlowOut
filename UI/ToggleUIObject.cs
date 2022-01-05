using UnityEngine;

public class ToggleUIObject : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    public void ToggleObject()
    {
        obj.SetActive(!obj.activeSelf);
    }
}
