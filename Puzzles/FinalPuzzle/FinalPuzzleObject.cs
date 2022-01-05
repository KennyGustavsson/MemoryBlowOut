using UnityEngine;

public class FinalPuzzleObject : MonoBehaviour
{
	public int value;
	public Reaction pickupReaction = null;

	public void TriggerReaction()
	{
		if(pickupReaction != null)
			pickupReaction.TriggerReaction();
	}
}
