using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // The time that the level starts with
    [SerializeField] private int startingMinutes = 0;
    [SerializeField] private int startingSeconds = 0;
    static private int minutes = 0;
    static private int seconds = 0;
    private string strTime = "0:0";
    // The time accumulated from dt
    private float dtCounter = 0.0f;

    // Static time overview
    static public float timeOverview = 1.0f;
    private void Awake()
    {
        // Set the default mituest and minutes
        minutes = startingMinutes;
        seconds = startingSeconds;

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
    // Get score calculated from the time remaining
    static public float GetScore()
    {
        float answr = 0.0f;
        answr += (float)minutes;
        answr += (float)seconds / 60;
        // Round to 2 dec places
        answr *= 100;
        answr = Mathf.Round(answr);
        answr /= 100;
        return answr;
    }
}

