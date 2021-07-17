using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI = null;
    [SerializeField] private GameObject gameplayUI = null;
    // Players object
    [SerializeField] private PlayersManager players = null;
    // Current last check point
    private int lastCheckpointIndex = 0;
    [SerializeField] private CheckpointManager[] checkpoints;
    // Victory location
    [SerializeField] private GameObject finishLine = null;
    // If pause menu is open
    public static bool isPaused = false;

    private void Awake()
    {
        // Set the pause menu to inactive
        pauseMenuUI.SetActive(false);
        // Set the initial checkpoint to reached
        checkpoints[lastCheckpointIndex].SetIsReached(true);

        // Save the level's name
        ScenesNavigation.SaveLastLevelPlayed(SceneManager.GetActiveScene().name);
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

        // If players die reload them to the last checkpoint
        if (arePLayersFallen())
        {
            CheckpointReturn();
        }

        // Update the checkpoint if players have passed it
        if (lastCheckpointIndex < checkpoints.Length - 1) // the last checkpoint doesn't need to be checked
        {
            if (players.GetPosition().x >= checkpoints[lastCheckpointIndex + 1].transform.position.x)
            {
                lastCheckpointIndex++;
                checkpoints[lastCheckpointIndex].SetIsReached(true);
            }
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
        players.ChangePosition(checkpoints[lastCheckpointIndex].transform.position);
    }
    // Returns true if the players reach the final destination
    private bool isWon()
    {
        if (players.GetPosition().x >= finishLine.transform.position.x - 1 && 
        players.GetPosition().x <= finishLine.transform.position.x + 1)
        {
            return true;
        }
        return false;
    }
    // Sets the isLost to true
    private bool arePLayersFallen()
    {
        if (players.GetPosition().y <= -10.0f)
        {
            return true;
        }
        return false;
    }
}
