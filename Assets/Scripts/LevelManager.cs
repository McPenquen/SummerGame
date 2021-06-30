using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Name of the scene
    [Header("Level1")]
    [SerializeField] private GameObject pauseMenuUi = null;
    // Null instance of the manager
    public static LevelManager instance = null;
    public static bool isPaused = false;
   // public GameObject pauseMenuUI = null;
    private void Awake()
    {
        // Check if instance is null
        if (instance == null)
        {
            //Don't destroy the current game manager
            DontDestroyOnLoad(gameObject);

            //Set game manager instance to this
            instance = this;
        }
        // Check if current instance of game manager is equal to this game manager
        else if (instance != this)
        {
            //Destroy the game manager that is not the current game manager
            Destroy(gameObject);
        }
        pauseMenuUi.SetActive(false);
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
    }

    public void EnterMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    // Pause the game
    public void PauseGame()
    {
        // Pause the game and show the UI 
        isPaused = true;
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0;
    }

    // Resume the game
    public void ResumeGame()
    {
        // Unpause the game and hide the UI 
        isPaused = false;
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1;
    }
    // pause menu inspired from: https://www.youtube.com/watch?v=JivuXdrIHK0&t=354s&ab_channel=Brackeys 
}
