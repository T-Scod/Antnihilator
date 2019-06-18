using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject gameOverCanvas;
    public GameObject pauseCanvas;

    private InsectNest m_insectNest;
    private bool m_gameOver = false;
    private bool m_inMenu = true;

    private void Awake()
    {
        m_insectNest = FindObjectOfType<InsectNest>();
    }

    private void Update()
    {
        if (!m_gameOver && OVRInput.GetDown(OVRInput.Button.Back))
        {
            if (m_inMenu)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!m_inMenu && !m_gameOver && int.Parse(FindObjectOfType<UnityEngine.UI.Text>().text) <= 0)
        {
            GameOver();
        }
    }

    public void Pause()
    {
        m_inMenu = true;
        pauseCanvas.SetActive(true);
        m_insectNest.SetPause(true);
    }

    public void Resume()
    {
        m_inMenu = false;
        pauseCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        m_insectNest.SetPause(false);
    }

    private void GameOver()
    {
        m_gameOver = true;
        m_inMenu = true;
        gameOverCanvas.SetActive(true);
    }
}