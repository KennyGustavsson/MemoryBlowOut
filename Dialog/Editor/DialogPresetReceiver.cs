using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
public class DialogPresetReceiver : PresetSelectorReceiver
{
    private Preset initValues;

    private Dialog currentSettings;

    public void Init(Dialog cur)
    {
        currentSettings = cur;
        initValues = new Preset(cur);
    }

    public override void OnSelectionChanged(Preset selection)
    {
        if (selection != null)
        {
            string dialogText = currentSettings.dialogText;
            float dialogTime = currentSettings.dialogTime;
            currentSettings.dialogText = "";
            currentSettings.dialogTime = 10;
            selection.ApplyTo(currentSettings);
            currentSettings.dialogTime = dialogTime;
            currentSettings.dialogText = dialogText;
        }
        else
        {
            initValues.ApplyTo(currentSettings);
        }
    }

    public override void OnSelectionClosed(Preset selection)
    {
        OnSelectionChanged(selection);
        DestroyImmediate(this);
    }
}