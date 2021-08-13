using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI = null;
    [SerializeField] private GameObject gameplayUI = null;
    [SerializeField] private GameObject optionsScene = null;
    // Players object
    [SerializeField] private PlayersManager players = null;
    // Current last check point
    private int lastCheckpointIndex = 0;
    [SerializeField] private CheckpointManager[] checkpoints;
    // Victory location
    [SerializeField] private GameObject finishLine = null;
    // Victory boolean
    private bool isWon = false;
    // If pause menu is open
    public static bool isPaused = false;
    // The victory scene delay counter
    private float victoryDelayCounter = 0.0f;
    // Bool if the ufo engine sound has been triggered
    private bool isPlayingUfoEngineSound = false;

    private void Awake()
    {
        // Set the pause menu to inactive
        pauseMenuUI.SetActive(false);
        // Set the initial checkpoint to reached
        checkpoints[lastCheckpointIndex].SetIsReached(true);

        // Save the level's name
        ScenesNavigation.SaveLastLevelPlayed(SceneManager.GetActiveScene().name);

        // Save the max time -> as the max score
        ScenesNavigation.SaveMaxScore(Timer.GetScore());
    }

    private void  Update() 
    {
        // Pressing esc or controller back button pauses and unpauses the game
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            // When it is paused and options are on return to the paused menu
            if (isPaused && optionsScene.active == true)
            {
                OptionsToPause();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // If players reach the final destination they won
        FinishLineVictoryCheck();
        if (isWon)
        {
            // Activate sound only once
            if (victoryDelayCounter == 0)
            {
                AkSoundEngine.PostEvent("play_beam_up",gameObject);
                // Start the spaceship animation
                SpaceshipManager.SetIsWon(true);
            }
            // Save the time -> as score
            ScenesNavigation.SaveScore(Timer.GetScore());
            // Wait until the sound finishes to change scene
            victoryDelayCounter += Time.deltaTime;
            if (victoryDelayCounter >= 2.0f )
            {
                // Stop all current sounds and replay the default background noise
                AkSoundEngine.StopAll();
                AkSoundEngine.PostEvent("play_start_up", UnityEngine.GameObject.Find("WwiseGlobal"));
                // Make sure the players are active for next time
                players.gameObject.SetActive(true);
                SceneManager.LoadScene("Victory");
            }
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
        // Stop all current sounds and replay the default background noise
        AkSoundEngine.StopAll();
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        AkSoundEngine.PostEvent("play_start_up", UnityEngine.GameObject.Find("WwiseGlobal"));
        SceneManager.LoadScene("Menu");
    }

    // Pause the game
    public void PauseGame()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        // Pause the game, show the pause menu UI and hide the gameplay UI
        isPaused = true;
        pauseMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        Time.timeScale = 0;
    }

    // Resume the game
    public void ResumeGame()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        // Unpause the game, hide the pause menu UI and show the gameplay UI
        isPaused = false;
        pauseMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        Time.timeScale = 1;
    }
    // pause menu inspired from: https://www.youtube.com/watch?v=JivuXdrIHK0&t=354s&ab_channel=Brackeys 
    
    // Enter Options scene
    public void EnterOptions()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        optionsScene.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    // Return from Options to Paused menu
    public void OptionsToPause()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        pauseMenuUI.SetActive(true);
        optionsScene.SetActive(false);
    }

    // Return players to the latest checkpoint
    private void CheckpointReturn()
    {
        players.ChangePosition(checkpoints[lastCheckpointIndex].transform.position);
    }
    // Sets isWon to true if the players reach the final destination
    private void FinishLineVictoryCheck()
    {
        if (players.GetPosition().x >= finishLine.transform.position.x - 1 && 
        players.GetPosition().x <= finishLine.transform.position.x + 1)
        {
            isWon = true;
        }
        else
        {
          isWon = false;  
        }
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
    // Go to the game over scene = loss game
    static public void GameOver()
    {
        // Stop all current sounds and replay the default background noise
        AkSoundEngine.StopAll();
        AkSoundEngine.PostEvent("play_start_up", UnityEngine.GameObject.Find("WwiseGlobal"));
         SceneManager.LoadScene("GameOver");
    }
}
