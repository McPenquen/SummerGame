using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private int minutes = 0;
    [SerializeField] private int seconds = 0;
    private string strTime = "0:0";
    // The time accumulated from dt
    private float dtCounter = 0.0f;

    private void Awake()
    {
        // Transform seconds into minutes 
        IncreaseSecsToMins();
        
        // Update the string time
        UpdateTimeStr();
    }
    void Update()
    {
        // Update the timer of dt
        dtCounter += Time.deltaTime;

        // When it is over a second
        if (dtCounter >= 1.0f)
        {
            seconds--;
            dtCounter -= 1.0f;
            // Check if the seconds are 0
            DecreaseSecsToMins();
            // Update the new string to the text
            UpdateTimeStr();   
        }

        // Lose the game if the time has reached 0
        if (minutes == 0 && seconds == 0)
        {
            LevelManager.GameOver();
        }
    }

    // Convert seconds into minutes = seconds be over 60
    private void IncreaseSecsToMins()
    {
        if (seconds >= 60)
        {
            while (seconds >= 60)
            {
                seconds -= 60;
                minutes++;
            }
        }
    }

    // Convert seconds into minutes = seconds be 0
    private void DecreaseSecsToMins()
    {
        if (seconds < 0)
        {
            while (seconds < 0)
            {
                seconds += 60;
                minutes--;
            }
        }
    }

    // Update the string to correspond to the saved values
    private void UpdateTimeStr()
    {
        strTime = minutes.ToString() + ":" + seconds.ToString();
        GetComponent<TMPro.TextMeshProUGUI>().text = strTime;
    }
}
