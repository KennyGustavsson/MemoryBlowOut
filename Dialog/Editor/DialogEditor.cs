using UnityEngine;
using  UnityEditor;
using UnityEditor.Presets;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor
{
    private Dialog dialog;

    private new SerializedObject serializedObject;

    private DialogPresetReceiver dialogPresetReceiver;
    private string presetName;

    private string dialogText;
    private float dialogTime;

    private void OnEnable()
    {
        presetName = "Preset Name";
        Selection.selectionChanged += SelectionChanged;
    }

    private void SelectionChanged()
    {
        foreach (Object obj in Selection.objects)
        {
            if (obj.GetType() == typeof(Dialog))
            {
                dialog = (Dialog) obj;

                serializedObject = new SerializedObject(dialog);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        
        presetName = EditorGUILayout.TextField(presetName);

        if (GUILayout.Button("Save Preset"))
        {
            OnSavePresetClicked();
            GUIUtility.ExitGUI();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Load Preset"))
        {
            OnLoadPresetClicked();
            GUIUtility.ExitGUI();
        }
    }

    private void OnLoadPresetClicked()
    {
        DialogPresetReceiver presetReceiver = CreateInstance<DialogPresetReceiver>();
        dialogText = dialog.dialogText;
        dialogTime = dialog.dialogTime;
        presetReceiver.Init(dialog);
        PresetSelector.ShowSelector(dialog, null, false, presetReceiver);
    }
    private void OnSavePresetClicked()
    {
        presetName = presetName.Trim();

        if (string.IsNullOrEmpty(presetName))
        {
            EditorUtility.DisplayDialog("Unable to save preset", "Please specify a valid preset name", "close");
            return;
        }
        else
        {
            CreatePresetAsset(dialog, presetName);
            EditorUtility.DisplayDialog("Preset Saved!", "The preset was saved Successfully", "Close");
            return;
        }
        
    }
    
    private static void CreatePresetAsset(Object source, string presName)
    {
        Preset preset = new Preset(source);
        AssetDatabase.CreateAsset(preset, "Assets/Scripts/Dialog/Presets/" + presName + ".preset");
    }

}
