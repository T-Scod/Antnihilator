using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to give functionality to buttons.
/// </summary>
public class UIManager : MonoBehaviour
{
    private int m_currentState = 0;

    /// <summary>
    /// Loads the menu scene.
    /// </summary>
    public void Menu()
    {
        m_currentState = 0;
        SceneManager.LoadScene(0);
    }

    public void Next()
    {
        m_currentState++;
        SceneManager.LoadScene(m_currentState);
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    public void Play()
    {
        m_currentState = 1;
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