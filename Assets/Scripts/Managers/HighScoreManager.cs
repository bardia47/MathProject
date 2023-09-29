using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager manager;
    public List<int> scores = new List<int>();

    private static string HIGH_SCORE_KEY = "highScores";
    private void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
            return;
        }
        manager = this;
    }
    private void Start()
    {
        LoadScores();
    }
    public void SaveScore(int newScore) {
        scores.Add(newScore);
        scores.Sort();
        scores.Reverse();
        string showScore = string.Join(",", scores);
        PlayerPrefs.SetString(HIGH_SCORE_KEY, showScore);
        UIManager.manager.ToggleHighScoreTable();
    }

    public void LoadScores() {
        scores.Clear();
        if (PlayerPrefs.HasKey(HIGH_SCORE_KEY)) {
            
                scores = PlayerPrefs.GetString(HIGH_SCORE_KEY).Split(",").Select(Int32.Parse).ToList(); ;
        }
    }

}
