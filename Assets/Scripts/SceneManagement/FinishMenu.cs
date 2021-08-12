using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    // Go to main menu
    public void GetToMainMenu()
    {
        // Stop all current sounds and replay the default background noise
        AkSoundEngine.StopAll();
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        AkSoundEngine.PostEvent("play_start_up", UnityEngine.GameObject.Find("WwiseGlobal"));
        SceneManager.LoadScene("Menu");
    }
    // Replay the last played level
    public void ReplayLastLevel()
    {
        // Stop all current sounds and replay the default background noise
        AkSoundEngine.StopAll();
        // Play confirm sound
        AkSoundEngine.PostEvent("play_ui_confirm", UnityEngine.GameObject.Find("ScenesNavigator"));
        AkSoundEngine.PostEvent("play_start_up", UnityEngine.GameObject.Find("WwiseGlobal"));
        SceneManager.LoadScene(ScenesNavigation.GetLastLevelName());
    }
}
