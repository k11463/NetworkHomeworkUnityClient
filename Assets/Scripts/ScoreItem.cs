using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public UnityEngine.UI.Text ScoreValueText;

    public void SetScore(string name, int score)
    {
        ScoreValueText.text = name + ", " + score.ToString();
    }
}
