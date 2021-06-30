using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Name of the scene
    [Header("MainMenu")]
    // Null instance of the manager
    public static MenuManager instance = null;
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
    }

    // Play the game with index 1
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Close the application
    public void QuitGame() 
    {
        Application.Quit();
    }
}
