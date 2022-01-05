using UnityEngine;

public class EnableAnimatorReaction : Reaction
{
	[SerializeField] private Animator component = default;
	[SerializeField] private new bool enabled = default;
	
	public override void TriggerReaction()
	{
		component.enabled = enabled;
	}
}
