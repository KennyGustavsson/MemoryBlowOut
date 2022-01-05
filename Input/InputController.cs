using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/// <summary>
/// Controls all non ui input and triggers the appropriate action for the input.
/// </summary>
public class InputController
{
	// Mouse click input
	// Screen to ray, gameObject

	public bool allowInput = true;
	//tracks if the mouse button was first pressed down when it was not over UI.
	private bool moveDownInput = false;
	
	public void Update()
	{
		if (GetEscapeButton())
			OnPause();
		if (GetLeftMouseUp())
			moveDownInput = false;
		
		if(!allowInput || EventSystem.current.IsPointerOverGameObject() || GameManager.Instance.isPaused) return;

		if (GetLeftMouseDown())
		{
			moveDownInput = true;
		}
		
		if (Camera.main)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			ObjectAndPositionRaycast(ray);
		}
		else
		{
			Debug.LogError("No Main Camera Was Found");
		}
	}

	/// <summary>
	/// Returns true if the left mouse is pressed down during the frame that it is called.
	/// </summary>
	/// <returns></returns>
	private static bool GetLeftMouseDown()
	{
		return Input.GetButtonDown("LeftMouseButton");
	}
	
	/// <summary>
	/// Returns true if the left mouse is let go during the frame that it is called.
	/// </summary>
	/// <returns></returns>
	private static bool GetLeftMouseUp()
	{
		return Input.GetButtonDown("LeftMouseButton");
	}

	/// <summary>
	/// Returns true if the left mouse is being held the frame that it is called.
	/// </summary>
	/// <returns></returns>
	private static bool GetLeftMouseButton()
	{
		return Input.GetButton("LeftMouseButton");
	}

	/// <summary>
	/// Returns true if the escape key is being pressed down during the frame that it is called.
	/// </summary>
	/// <returns></returns>
	private static bool GetEscapeButton()
	{
		return Input.GetKeyDown(KeyCode.Escape);
	}
	
	/// <summary>
	/// sends a raycast and trigger various reactions depending on the input. This function can be split into more than
	/// one function to make things more clear.
	/// </summary>
	/// <param name="ray"></param>
	private void ObjectAndPositionRaycast(Ray ray)
	{
		if (!Physics.Raycast(ray, out RaycastHit hit)) return;
		
		if (hit.collider.gameObject.layer == 8)
		{
			EventManager.onMouseClickWalkable(hit.point);
		}
		else
		{
			IInteractable interaction = hit.collider.GetComponent<IInteractable>();

			if (interaction != null)
			{
				GameManager.Instance.ChangeHover(MouseCursor.HoverState.Interactable);
				if (!GetLeftMouseDown()) return;
				
				EventManager.onMouseClickObject(interaction);
			}
			else
			{
				if (GetLeftMouseButton() && moveDownInput)
				{
					EventManager.onMouseClickWalkable(hit.point);
				}
				
				if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1, NavMesh.AllAreas))
				{
					if(Vector3.Distance(navHit.position, hit.point) < 0.2f)
						GameManager.Instance.ChangeHover(MouseCursor.HoverState.Walkable);
					return;
				}
				
				GameManager.Instance.ChangeHover(MouseCursor.HoverState.Normal);
			}
		}
	}

	/// <summary>
	/// Receives the pause event.
	/// </summary>
	private void OnPause()
	{
		EventManager.OnPause(!GameManager.Instance.isPaused);
	}
}
