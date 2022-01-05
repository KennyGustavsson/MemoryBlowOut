using UnityEngine;

/// <summary>
/// A visual Representation of the <see cref="switchBoard"/> using lights.
/// </summary>
public class SwitchBoardLights : MonoBehaviour
{
    [Header("SwitchBoard")]
    [SerializeField] private SwitchBoard switchBoard;

    [Header("Lights")]
    [SerializeField] private Renderer[] lightRenderers;
    
    [Header("Materials")]
    [SerializeField] private Color offColor;
    [ColorUsage(true,true)] [SerializeField] private Color offColorEmission;
    [SerializeField] private Color onColor;
    [ColorUsage(true,true)] [SerializeField] private Color onColorEmission;
    [SerializeField] private Color overLoadColor;
    [ColorUsage(true,true)] [SerializeField] private Color overLoadColorEmission;
    
    private void Awake()
    {
        switchBoard.onVoltageChanged += OnVoltageChanged;
        }

    private void OnVoltageChanged(float voltage)
    {
        UpdateLights(voltage);
    }

    private void UpdateLights(float voltage)
    {
        for (int i = 0; i < lightRenderers.Length; i++)
        {
            if (voltage > switchBoard.maxCapacity)
            {
                lightRenderers[i].material.SetColor("Color_894298EC", overLoadColorEmission);
                lightRenderers[i].material.SetColor("Color_103EEC6E", overLoadColor);
                lightRenderers[i].material.SetFloat("Boolean_E669A592", 1);

                continue;
            }

            if (voltage > switchBoard.maxCapacity / lightRenderers.Length * i + 1 )
            {
                lightRenderers[i].material.SetColor("Color_894298EC", onColorEmission);
                lightRenderers[i].material.SetColor("Color_103EEC6E", onColor);
                lightRenderers[i].material.SetFloat("Boolean_E669A592", 1);
            }
            else
            {
                lightRenderers[i].material.SetColor("Color_894298EC", offColorEmission);
                lightRenderers[i].material.SetColor("Color_103EEC6E", offColor);
                lightRenderers[i].material.SetFloat("Boolean_E669A592", 0);

            }

        }
    }
}
