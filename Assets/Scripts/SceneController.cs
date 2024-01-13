using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    private string currentSceneName;

    void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void ResumeGame()
    {
        SceneManager.LoadScene(currentSceneName);
    }

    public void SwitchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
