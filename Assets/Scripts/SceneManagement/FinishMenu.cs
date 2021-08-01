using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    // Go to main menu
    public void GetToMainMenu()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        SceneManager.LoadScene("Menu");
    }
    // Replay the last played level
    public void ReplayLastLevel()
    {
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        SceneManager.LoadScene(ScenesNavigation.GetLastLevelName());
    }
}
