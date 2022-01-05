using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayToolTip : MonoBehaviour
{

    [SerializeField] private RectTransform toolTipBox  = default;
    [SerializeField] private TMP_Text toolTipDescriptionText  = default;
    [SerializeField] private TMP_Text toolTipNameText  = default;

    public void Display(string itemName, string itemDescription, Vector2 position)
    {
        toolTipBox.gameObject.SetActive(true);
        toolTipBox.position = position;
        toolTipDescriptionText.text = itemDescription;
        toolTipNameText.text = itemName;
    }

    public void Hide()
    {
        toolTipBox.gameObject.SetActive(false);
    }

}
