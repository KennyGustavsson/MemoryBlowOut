using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[SerializeField] private Color imageColor = Color.black;
	[SerializeField] private Color textColor = Color.white;
	
	[SerializeField] private float fadeSpeed = 1f;
	[SerializeField] private float delayAfterFade = 1f;
	[SerializeField] private TextMeshProUGUI text = null;
	[SerializeField] private Image image = null;

	public void ActivateLoadingScreen(Action action)
	{
		StopAllCoroutines();
		StartCoroutine(FadeInLoadingScreen(action));
	}

	private IEnumerator FadeInLoadingScreen(Action action)
	{
		float time = 0;

		while (image.color.a < 1f)
		{
			image.color = Color.Lerp(new Color(imageColor.r, imageColor.g, imageColor.b, 0), imageColor, time * fadeSpeed);
			text.color = Color.Lerp(new Color(textColor.r, textColor.g, textColor.b, 0), textColor, time * fadeSpeed);
			
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
		}

		image.color = Color.black;
		text.color = Color.white;
		
		yield return new WaitForSeconds(delayAfterFade);
		
		action?.Invoke();
	}
}
