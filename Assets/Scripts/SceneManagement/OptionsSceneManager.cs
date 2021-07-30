using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSceneManager : MonoBehaviour
{
    // Null instance of the manager
    public static OptionsSceneManager instance = null;
    // The scene to return to by pressing back
    [SerializeField] public GameObject backScene = null;
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

        transform.gameObject.SetActive(false);
    }

    // Back to the previous scene
    public void Back()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        backScene.SetActive(true);
        transform.gameObject.SetActive(false);
    }
}
