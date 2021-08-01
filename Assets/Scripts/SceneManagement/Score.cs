using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject stars = null;
    private float maxScore = 0;
    private float gainedScore = 0;
    void Start()
    {
        // Save the gained and maximum score from the last level played
        gainedScore = ScenesNavigation.GetScore();
        maxScore = ScenesNavigation.GetMaxScore();
        // Show the gained score
        string scoreStr = "Score: " + gainedScore.ToString();
        GetComponent<TMPro.TextMeshProUGUI>().text = scoreStr;
        // Setup the stars based on the score
        float percentageScore = gainedScore / maxScore * 100;
        // Under 40 it's 1 star
        if (percentageScore <= 40)
        {
            stars.transform.GetChild(0).gameObject.SetActive(true);
            stars.transform.GetChild(1).gameObject.SetActive(false);
            stars.transform.GetChild(2).gameObject.SetActive(false);
        }
        // Under 80 it's 2 stars
        else if (percentageScore <= 80)
        {
            stars.transform.GetChild(0).gameObject.SetActive(false);
            stars.transform.GetChild(1).gameObject.SetActive(true);
            stars.transform.GetChild(2).gameObject.SetActive(false);
        }
        // above 80 it's 3 stars
        else
        {
            stars.transform.GetChild(0).gameObject.SetActive(false);
            stars.transform.GetChild(1).gameObject.SetActive(false);
            stars.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}
