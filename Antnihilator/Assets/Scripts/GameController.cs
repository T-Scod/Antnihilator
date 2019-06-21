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

    /// <summary>
    /// Reference to the insect spawnner.
    /// </summary>
    private InsectNest m_insectNest;
    /// <summary>
    /// Determines if the game is over.
    /// </summary>
    private bool m_gameOver = false;

    /// <summary>
    /// Gets the insect nest component from the scene.
    /// </summary>
    private void Awake()
    {
        m_insectNest = FindObjectOfType<InsectNest>();
    }

    /// <summary>
    /// Checks if the game is over each frame.
    /// </summary>
    private void Update()
    {
        // checks if the game is nopt yet over and if the player has run out of health
        if (!m_gameOver && int.Parse(FindObjectOfType<UnityEngine.UI.Text>().text) <= 0)
        {
            // ensures that the health does not become negative.
            FindObjectOfType<UnityEngine.UI.Text>().text = "0";
            // moves the game into an end state
            GameOver();
        }
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
    }
}