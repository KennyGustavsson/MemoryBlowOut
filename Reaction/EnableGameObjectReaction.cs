using UnityEngine;

public class EnableGameObjectReaction : Reaction
{
	[SerializeField] private GameObject obj = default;
	[SerializeField] private bool setActive = default;

	private void Awake()
	{
		if (obj == null)
			obj = gameObject;
	}

	public override void TriggerReaction()
	{
		Debug.Log(obj + " = " + setActive);
		obj.SetActive(setActive);
	}
}
