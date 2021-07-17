using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    // Go to main menu
    public void GetToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    // Replay the last played level
    public void ReplayLastLevel()
    {
        SceneManager.LoadScene(ScenesNavigation.GetLastLevelName());
    }
}
