using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HighScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;
    public void setScore(int score) {
        scoreText.text = $"{transform.GetSiblingIndex() + 1} . {score}";
    }
}
