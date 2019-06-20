using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject gameOverCanvas;

    private InsectNest m_insectNest;
    private bool m_gameOver = false;

    private void Awake()
    {
        m_insectNest = FindObjectOfType<InsectNest>();
    }

    private void Update()
    {
        if (!m_gameOver && int.Parse(FindObjectOfType<UnityEngine.UI.Text>().text) <= 0)
        {
            FindObjectOfType<UnityEngine.UI.Text>().text = "0";
            GameOver();
        }
    }

    private void GameOver()
    {
        m_gameOver = true;
        m_insectNest.SetPause(true);
        gameOverCanvas.SetActive(true);
    }
}