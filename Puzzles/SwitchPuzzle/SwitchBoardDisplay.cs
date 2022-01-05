using UnityEngine;
using UnityEngine.UI;

public class SwitchBoardDisplay : MonoBehaviour
{
    [SerializeField]private Text voltageText  = null;
    [SerializeField]private SwitchBoard switchBoard  = null;

    private void OnEnable()
    {
        switchBoard.onVoltageChanged += ChangeScreenText;
    }

    private void OnDisable()
    {
        switchBoard.onVoltageChanged -= ChangeScreenText;
    }

    private void ChangeScreenText(float voltage)
    {
        voltageText.text = voltage.ToString();
    }
}
