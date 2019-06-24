using UnityEngine;

/// <summary>
/// Handles the game loop.
/// </summary>
public class GameController : MonoBehaviour
{
    /// <summary>
    /// Reference to the game over canvas object.
    /// </summary>
    [Tooltip("The game over canvas that becomes active when the game ends.")]
    public GameObject gameOverCanvas;
    //public GameObject gameWinCanvas;
    /// <summary>
    /// Reference to the OVR Gaze Pointer.
    /// </summary>
    [Tooltip("Reference to the OVR Gaze Pointer.")]
    public GameObject ovrGazePointer;

    /// <summary>
    /// Determines if the game was won.
    /// </summary>
    [HideInInspector]
    public bool gameWin = false;

    /// <summary>
    /// Reference to the insect spawnner.
    /// </summary>
    private InsectNest m_insectNest;
    /// <summary>
    /// Determines if the game is over.
    /// </summary>
    private bool m_gameOver = false;
    /// <summary>
    /// Reference to the UI manager to gain access to player health.
    /// </summary>
    private UIManager m_uiManager;

    /// <summary>
    /// Gets the insect nest component from the scene.
    /// </summary>
    private void Awake()
    {
        m_insectNest = FindObjectOfType<InsectNest>();
        m_uiManager = FindObjectOfType<UIManager>();
    }

    /// <summary>
    /// Checks if the game is over each frame.
    /// </summary>
    private void Update()
    {
        // checks if the game is not yet over and if the player has run out of health
        if (!m_gameOver && m_uiManager.health <= 0)
        {
            // moves the game into an end state
            GameOver();
        }
        if (gameWin && m_uiManager.health > 0)
        {
            GameWin();
        }
    }

    private void GameWin()
    {
        // sets the status of the game to over
        gameWin = true;
        // deactivates the insects and stops spawnning
        m_insectNest.SetPause(true);
        // makes the game over canvas visible
        //gameWinCanvas.SetActive(true);
        ovrGazePointer.SetActive(true);
        FindObjectOfType<LineRenderer>().enabled = false;
    }

    /// <summary>
    /// Moves the game into the end state.
    /// </summary>
    private void GameOver()
    {
        // sets the status of the game to over
        m_gameOver = true;
        // deactivates the insects and stops spawnning
        m_insectNest.SetPause(true);
        // makes the game over canvas visible
        gameOverCanvas.SetActive(true);
        ovrGazePointer.SetActive(true);
        FindObjectOfType<LineRenderer>().enabled = false;
    }
}