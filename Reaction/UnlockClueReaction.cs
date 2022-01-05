using UnityEngine;

public class UnlockClueReaction : Reaction
{
	[SerializeField] private int clueIndex = 0;
	
	public override void TriggerReaction()
	{
		EventManager.OnUnlockClue(clueIndex);
	}
}
