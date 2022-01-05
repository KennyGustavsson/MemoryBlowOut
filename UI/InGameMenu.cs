using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
	[SerializeField] private int menuSceneIndex = 0;
	[SerializeField] private int restartSceneIndex = 1;
	[SerializeField] private GameObject menuObj = null;
	
	private void OnEnable()
	{
		EventManager.onPause += OnPause;
	}

	private void OnDisable()
	{
		EventManager.onPause -= OnPause;
	}

	private void OnPause(bool isPaused)
	{
		if(isPaused)
			Pause();
		else
			ResumeEvent();
	}

	private void ResumeEvent()
	{
		menuObj.SetActive(false);
		Time.timeScale = 1;
	}
	
	public void Resume()
	{ 
		EventManager.OnPause(false);
	}

	private void Pause()
	{
		menuObj.SetActive(true);
		Time.timeScale = 0;
	}

	public void PauseGame()
	{
		EventManager.OnPause(!GameManager.Instance.isPaused);
	}
	
	public void ExitToMainMenu()
	{
		Resume();
		SceneManager.LoadScene(menuSceneIndex);
	}

	public void ExitToDesktop()
	{
		Application.Quit();
	}

	public void RestartScene()
	{
		SceneManager.LoadScene(restartSceneIndex);
	}
	
	public void DeleteSaveFile()
	{
		string destination = Application.persistentDataPath + "/save.dat";

		if (File.Exists(destination))
		{
			File.Delete(destination);
		}
	}
}
