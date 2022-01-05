using UnityEngine;

public class FinalPuzzleAnswerSlot : FinalPuzzleSlotBase
{
	[SerializeField] private int correctValue = default;
	
	public override bool CheckAnswer()
	{
		if (puzzleObj == null)
			return false;
		
		return correctValue == puzzleObj.GetComponent<FinalPuzzleObject>().value;
	}
}
