using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesNavigation : MonoBehaviour
{
    // The name of the last played level scene
    static private string lastLevelName = ""; 
    // Null instance of the manager
    public static ScenesNavigation instance = null;
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

    // Save the name of the scene of the last played level
    static public void SaveLastLevelPlayed(string levelName)
    {
        lastLevelName = levelName;
    }

    // Get the last level's name
    static public string GetLastLevelName()
    {
        return lastLevelName;
    }
}
