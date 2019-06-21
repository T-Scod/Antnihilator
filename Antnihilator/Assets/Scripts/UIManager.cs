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
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    public void Play()
    {
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