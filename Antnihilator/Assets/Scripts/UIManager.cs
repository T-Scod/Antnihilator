using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Resume()
    {
        FindObjectOfType<GameController>().Resume();
    }
    public void Exit()
    {
        Application.Quit();
    }
}