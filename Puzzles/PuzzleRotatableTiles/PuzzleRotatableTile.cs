using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class PuzzleRotatableTile : MonoBehaviour
{
	[Header("Position In Grid")]
	public Vector2Int myPos;
	[NonSerialized] public GameObject obj;
	
	[Header("Directions")]
	public Vector2Int firstDir = Vector2Int.right;
	public Vector2Int secondDir = Vector2Int.left;

	[Header("Kill tile")] 
	public bool isKillTile = false;
	
	public void Init(Vector2Int position, GameObject obj)
	{
		myPos = position;
		this.obj = obj;
		
		// Check the objs rotation and get the right directions
	}
	
#if UNITY_EDITOR
	private void Update()
	{
		if (isKillTile)
		{
			// Debug.DrawRay(transform.position + Vector3.left * 0.02f, transform.forward * 0.05f, Color.black);
			// Debug.DrawRay(transform.position + Vector3.left * 0.02f, -transform.forward * 0.05f, Color.black);
			// Debug.DrawRay(transform.position + Vector3.left * 0.02f, transform.up * 0.05f, Color.black);
			// Debug.DrawRay(transform.position + Vector3.left * 0.02f, -transform.up * 0.05f, Color.black);
			return;
		}
		
		// Debug.DrawRay(transform.position + Vector3.up * 0.02f, (new Vector3(firstDir.x, 0, firstDir.y)) * 0.05f, Color.blue, 0.01f);
		// Debug.DrawRay(transform.position + Vector3.up * 0.02f, (new Vector3(secondDir.x, 0, secondDir.y)) * 0.05f, Color.red, 0.01f);
	}

	[DrawGizmo(GizmoType.Selected)]
	private void OnDrawGizmos()
	{
		var rotationMatrix = Matrix4x4.TRS(transform.position, transform.parent.transform.rotation, transform.lossyScale);
		Gizmos.matrix = rotationMatrix;

		if (isKillTile)
		{
			Gizmos.color = Color.black;
			
			Gizmos.DrawRay(Vector3.zero + Vector3.up * 0.1f, Vector3.forward * 0.05f);
			Gizmos.DrawRay(Vector3.zero + Vector3.up * 0.1f, Vector3.right * 0.05f);
			Gizmos.DrawRay(Vector3.zero + Vector3.up * 0.1f, Vector3.left * 0.05f);
			Gizmos.DrawRay(Vector3.zero + Vector3.up * 0.1f, Vector3.back * 0.05f);
			

			return;
		}
		
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(Vector3.up * 0.02f, (new Vector3(firstDir.x, 0, firstDir.y)) * 0.05f);
		
		Gizmos.color = Color.red;
		Gizmos.DrawRay(Vector3.up * 0.02f, (new Vector3(secondDir.x, 0, secondDir.y)) * 0.05f);
	}
#endif
}
