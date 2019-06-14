using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject menuCanvas;

    public void Play()
    {
        FindObjectOfType<InsectNest>().gameObject.SetActive(true);
        menuCanvas.SetActive(false);
        gameObject.SetActive(true);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Resume()
    {
        FindObjectOfType<GameController>().TogglePause();
    }
    public void Exit()
    {
        Application.Quit();
    }
}