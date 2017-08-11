using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour {
    public float AverageNecessaryTime;
    public float RatioForThreeStars;
    public float RatioForTwoStars;
    public float RatioForOneStar;
    public TimeController _timeController;
    public Text Scoretext;
    public int Score;
    public Text Timeplayed;
    public int TotalLevelsComplete;
    public Text Averagetime;


    public void Newlevel() {
        TotalLevelsComplete += 1;
    }

    public void Start() {
        TotalLevelsComplete = EventController.Instance.island - 1;
        _timeController = EventController.Instance.GetComponent<TimeController>();
        var avgTimePerLevel = _timeController.OverallTime / (TotalLevelsComplete+1);
        if (TotalLevelsComplete > 0) {
            Score = (int) ((600 * TotalLevelsComplete - _timeController.OverallTime) * 1.323253f);
        }

        Scoretext = GameObject.Find("Scoretext").GetComponent<Text>();
        Timeplayed = GameObject.Find("Texttime").GetComponent<Text>();
        Averagetime = GameObject.Find("Atimetext").GetComponent<Text>();

        Scoretext.text = "Score: " + Score;

        Timeplayed.text = "Time survived: " + (int) (_timeController.OverallTime) + " seconds";

        if (TotalLevelsComplete > 0)
            Averagetime.text = "Average Time per Level: " + Math.Round(avgTimePerLevel, 2) +
                               " seconds";
        else
            Averagetime.text = "No Levels completed!";
    }
}
