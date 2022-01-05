using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 0;
    [SerializeField] private LoadingScreen loadingScreen = null;
    
    public void NewGame()
    {
        if(loadingScreen != null)
            loadingScreen.ActivateLoadingScreen(SetUpNewGame);
        else
            SetUpNewGame();
            
    }

    private void SetUpNewGame()
    {
        string destination = Application.persistentDataPath + "/save.dat";

        if (File.Exists(destination))
        {
            File.Delete(destination);
        }

        SceneManager.LoadScene(sceneIndex);
    }

    public void ResumeGame()
    {
        if(loadingScreen != null)
            loadingScreen.ActivateLoadingScreen(LoadResumeGame);
        else
            LoadResumeGame();
    }

    private void LoadResumeGame()
    {
        SceneManager.LoadScene(sceneIndex);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
