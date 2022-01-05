using UnityEngine;

public class AudioUI : MonoBehaviour
{
	[SerializeField] private AK.Wwise.Event ClickAudioEvent  = default;
	[SerializeField] private AK.Wwise.Event HoverAudioEvent  = default;

	public void PlayClickAudio()
	{
		ClickAudioEvent.Post(gameObject);
	}

	public void PlayHoverAudio()
	{
		HoverAudioEvent.Post(gameObject);
	}
}
