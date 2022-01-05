using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InteractableBase), true)]
public class InteractableWaypoint : Editor
{
    private Transform interactableTransform;

    private SerializedObject so;
    private SerializedProperty propWaypointPosition;
    private SerializedProperty propWaypointRotation;


    private void OnEnable()
    {
        interactableTransform = (target as InteractableBase).transform;
        so = serializedObject;
        propWaypointPosition = so.FindProperty("waypointPosition");
        propWaypointRotation = so.FindProperty("waypointRotation");

        SceneView.duringSceneGui += DuringSceneGUI; 
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }
    

    private void DuringSceneGUI(SceneView sceneView)
    {
        

        so.Update();
        Vector3 handlePos = propWaypointPosition.vector3Value + interactableTransform.position;
        Quaternion handleRot = Quaternion.Euler(propWaypointRotation.vector3Value);

        Vector3 scale = new Vector3(1, 1, 1);
        
        Handles.TransformHandle(ref handlePos, ref handleRot, ref scale);
        
        propWaypointPosition.vector3Value = handlePos-interactableTransform.position;
        propWaypointRotation.vector3Value = handleRot.eulerAngles;
        so.ApplyModifiedProperties();
    }
    
    [DrawGizmo(GizmoType.Selected)]
    private static void DrawGizmos(InteractablePickup scr, GizmoType gizmoType)
    {
        Vector3 pos = scr.waypointPosition + scr.transform.position;
        Vector3 rot = scr.waypointRotation;
        Vector3 dir = new Vector3(Mathf.Sin(rot.y * Mathf.Deg2Rad),0, Mathf.Cos(rot.y * Mathf.Deg2Rad));
        
        Gizmos.DrawCube(Vector3.zero, Vector3.one * 10);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, dir);
    }




    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

}
