using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Name of the scene
    [Header("Menu")]
    // Null instance of the manager
    public static MenuManager instance = null;
    // Options Scene
    [SerializeField] public GameObject optionsScene = null;
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

    // Play level 1
    public void PlayGame()
    {
        // Stop all current sounds and replay the default background noise
        AkSoundEngine.StopAll();
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        //SceneManager.LoadScene("Level1");
        AkSoundEngine.PostEvent("play_start_up", UnityEngine.GameObject.Find("WwiseGlobal"));
        SceneManager.LoadScene("newLevel1");
    }

    // Close the application
    public void QuitGame() 
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        Application.Quit();
    }

    // Show the options scene
    public void ShowOptions()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        optionsScene.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}
