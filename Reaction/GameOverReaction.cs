using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverReaction : Reaction
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private int sceneIndex = 0;
    [SerializeField] private float sceneChangeDelay = 5f;

    public override void TriggerReaction()
    {
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(sceneChangeDelay);
        SceneManager.LoadScene(sceneIndex);
    }
}
