using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to give functionality to buttons.
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Collection of references to the leaf images.
    /// </summary>
    [Tooltip("Collection of references to the leaf images.")]
    public UnityEngine.UI.Image[] leaves;
    /// <summary>
    /// The current health of the player.
    /// </summary>
    [Tooltip("The current health of the player.")]
    public int health = 5;

    /// <summary>
    /// Decrements the amount of health the player has.
    /// </summary>
    /// <param name="amount">The amount of damage taken.</param>
    public void TakeDamage(int amount)
    {
        // decrements the health
        health -= amount;
        // checks if the player has run out of health
        if (health <= 0)
        {
            // disables all images
            for (int i = 0; i < 5; i++)
            {
                leaves[i].gameObject.SetActive(false);
            }
        }
        else
        {
            // checks if the image should be visible
            for (int i = 0; i < 5; i++)
            {
                if (i >= health)
                {
                    leaves[i].gameObject.SetActive(false);
                }
            }
        }
    }

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