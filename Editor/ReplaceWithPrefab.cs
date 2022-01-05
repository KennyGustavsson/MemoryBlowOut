using UnityEditor;
using UnityEngine;

public class ReplaceWithPrefab : EditorWindow
{
    [SerializeField] private GameObject prefab;
    
    [MenuItem("Tools/Replace with prefab")]
    static void CreateReplaceWithPrefab()
    {
        EditorWindow.GetWindow<ReplaceWithPrefab>();
    }

    private void OnGUI()
    {
        prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Replace"))
        {
            Replace();
        }
        
        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }

    private void Replace()
    {
        GameObject[] selection = Selection.gameObjects;

        for (int i = selection.Length - 1; i >= 0; i--)
        {
            GameObject selected = selection[i];
            var prefabType = PrefabUtility.GetPrefabType(prefab);
            GameObject newObject;
            
            if(prefabType == PrefabType.Prefab)
            {
                newObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            }
            else
            {
                newObject = Instantiate(prefab);
                newObject.name = prefab.name;
            }

            if (newObject == null)
            {
                Debug.LogError("Error instantiating prefab");
                break;
            }
            
            Undo.RegisterCreatedObjectUndo(newObject, "Replace with prefabs");

            newObject.transform.parent = selected.transform.parent;
            newObject.transform.localPosition = selected.transform.localPosition;
            newObject.transform.localRotation = selected.transform.localRotation;
            newObject.transform.localScale = selected.transform.localScale;
            newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
            
            Undo.DestroyObjectImmediate(selected);
        }
    }
}
