using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Name of the scene
    [Header("Level1")]
    [SerializeField] private GameObject pauseMenuUI = null;
    [SerializeField] private GameObject gameplayUI = null;
    // Players object
    [SerializeField] private GameObject players = null;
    // Current last check point
    [SerializeField] private GameObject lastCheckpoint = null;
    // Victory location
    [SerializeField] private GameObject finishLine = null;
    // If pause menu is open
    public static bool isPaused = false;
    // If the level is lost
    public static bool isLost = false;
    private void Awake()
    {
        pauseMenuUI.SetActive(false);
    }

    private void  Update() 
    {
        // Pressing esc pauses and unpauses the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // If players reach the final destination they won
        if (isWon())
        {
            SceneManager.LoadScene("Victory");
        }
        // If players fall they lose
        arePLayersFallen();
        // If players lose reload them to the last checkpoint
        if (isLost)
        {
            CheckpointReturn();
        }
    }

    public void EnterMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    // Pause the game
    public void PauseGame()
    {
        // Pause the game, show the pause menu UI and hide the gameplay UI
        isPaused = true;
        pauseMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        Time.timeScale = 0;
    }

    // Resume the game
    public void ResumeGame()
    {
        // Unpause the game, hide the pause menu UI and show the gameplay UI
        isPaused = false;
        pauseMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        Time.timeScale = 1;
    }
    // pause menu inspired from: https://www.youtube.com/watch?v=JivuXdrIHK0&t=354s&ab_channel=Brackeys 
    // Return players to the latest checkpoint
    private void CheckpointReturn()
    {
        players.transform.position = lastCheckpoint.transform.position;
        isLost = false;
    }
    // Returns true if the players reach the final destination
    private bool isWon()
    {
        if (players.transform.position == finishLine.transform.position)
        {
            return true;
        }
        return false;
    }
    // Sets the isLost to true
    private void arePLayersFallen()
    {
        if (players.transform.position.y < -20.0f)
        {
            isLost = true;
        }
    }
}
