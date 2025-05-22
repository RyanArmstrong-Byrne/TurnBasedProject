using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingPanel;
    public Image ProgressBar;
    public Text ProgressText;

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingPanel.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            ProgressBar.fillAmount = progress;
            ProgressText.text = $"{progress:P2}";
            yield return null;
        }
    }

    public void LoadLevelAsync(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    
    public void RestartLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadAsynchronously(currentScene));
    }
    public void ToMainMenu()
    {
        StartCoroutine(LoadAsynchronously(0));
    }
}