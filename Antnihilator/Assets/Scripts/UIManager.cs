using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to give functionality to buttons.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Loads the menu scene.
    /// </summary>
    public void Menu()
    {
        PlayerPrefs.SetInt("CurrentSceneIndex", 0);
        SceneManager.LoadScene(0);
    }

    public void Next()
    {
        int key = PlayerPrefs.GetInt("CurrentSceneIndex");
        key++;
        PlayerPrefs.SetInt("CurrentSceneIndex", key);
        SceneManager.LoadScene(key);
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    public void Play()
    {
        PlayerPrefs.SetInt("CurrentSceneIndex", 1);
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}