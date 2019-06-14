using UnityEngine;

public class GameController : MonoBehaviour
{
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
            TogglePause();
        }

        if (!m_inMenu && !m_gameOver && int.Parse(FindObjectOfType<UnityEngine.UI.Text>().text) <= 0)
        {
            GameOver();
        }
    }

    public void TogglePause()
    {
        m_inMenu = !m_inMenu;
        pauseCanvas.SetActive(m_inMenu);
        m_insectNest.SetPause(m_inMenu);
    }

    private void GameOver()
    {
        m_gameOver = true;
        m_inMenu = true;
        gameOverCanvas.SetActive(true);
    }
}