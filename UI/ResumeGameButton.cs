using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ResumeGameButton : MonoBehaviour
{
	private Button button = null;

	private void Awake()
	{
		string destination = Application.persistentDataPath + "/save.dat";
		button = GetComponent<Button>();

		if (!File.Exists(destination) && button != null)
		{
			button.interactable = false;
		}
	}
}
