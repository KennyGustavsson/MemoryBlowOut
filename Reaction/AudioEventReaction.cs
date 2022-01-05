using UnityEngine;

public class AudioEventReaction : Reaction
{
	[SerializeField] private AK.Wwise.Event audioEvent = default;
	
	[Tooltip("location for the audio, if empty it will play on this object")]
	[SerializeField] private GameObject audioLocation = default;
	
	public override void TriggerReaction()
	{
		audioEvent.Post(audioLocation == null ? gameObject : audioLocation);
	}
}
