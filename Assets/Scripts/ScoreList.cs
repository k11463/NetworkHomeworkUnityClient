using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreList : MonoBehaviour
{
    public ScoreItem[] Scoreitems;

    public void HideAllScoreItem()
    {
        foreach (var item in Scoreitems) 
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SetScore(int idx, string name, int score)
    {
        Scoreitems[idx].SetScore(name, score);
        Scoreitems[idx].gameObject.SetActive(true);
    }
}
