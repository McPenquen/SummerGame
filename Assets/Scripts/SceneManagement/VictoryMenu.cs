using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    // Go to main menu
    public void GetToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
