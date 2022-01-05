using UnityEditor;
using UnityEngine;

public class PlayerTriggerEditor : Editor
{
    [DrawGizmo(GizmoType.NonSelected)]
    private static void DrawGizmos(PlayerTrigger scr, GizmoType gizmoType)
    {
        BoxCollider collider = scr.GetComponent<BoxCollider>();
        if(!collider) return;
        
        Vector3 size = scr.transform.localScale;
        Vector3 center = collider.center;
        Vector3 colliderSize = collider.size;

        Vector3 offset = new Vector3(center.x * size.x, center.y * size.x,
            center.z * size.x);

        size = new Vector3(size.x * colliderSize.x, size.y * colliderSize.y, size.z * colliderSize.z);
        
        
        Gizmos.DrawWireCube(scr.transform.position + offset, size);
    }
}
