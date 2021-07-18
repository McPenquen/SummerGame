using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    void Start()
    {
        string scoreStr = "Score: " + ScenesNavigation.GetScore().ToString();
        GetComponent<TMPro.TextMeshProUGUI>().text = scoreStr;
    }
}
