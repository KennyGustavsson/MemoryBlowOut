using UnityEngine;

public abstract class FinalPuzzleSlotBase : MonoBehaviour
{
	public GameObject puzzleObj;
	
	[SerializeField] private Reaction itemPlacedReaction;
	[SerializeField] private Reaction itemRemovedReaction;

	public void ItemPlaced()
	{
		if (itemPlacedReaction) itemPlacedReaction.TriggerReaction();
	}
	
	public void ItemRemoved()
	{
		if (itemRemovedReaction) itemRemovedReaction.TriggerReaction();
	}


	private void Awake()
	{
		if (transform.childCount > 0){
			puzzleObj = transform.GetChild(0).gameObject;
		}
	}

	public abstract bool CheckAnswer();
}
