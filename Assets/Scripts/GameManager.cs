using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    private readonly int m_ScoreDelta = 999999999;
    private int m_currentScore = 0;

    public static GameManager Instance { get { return m_instance; } }
    public GameObject GameOverUI;
    public InputField PlayerNameIF;
    public ScoreList GameScoreList;
    public UnityEngine.UI.Text CurScoreText;

    public MyWebRequest WebRequest;

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }

        Time.timeScale = 1.0f;

        m_currentScore = 0;
        this.CurScoreText.text = m_currentScore.ToString();
        GameOverUI.SetActive(false);
    }
    public void GameOver()
    {
        GameOverUI.SetActive(true);

        // pause the game
        Time.timeScale = 0.0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore()
    {
        m_currentScore += m_ScoreDelta;
        this.CurScoreText.text = m_currentScore.ToString();
    }

    public void ShowUploadScore()
    {
        PlayerNameIF.transform.parent.gameObject.SetActive(true);
    }

    public void UploadScore()
    {
        string name = PlayerNameIF.text;
        if (name == "")
            name = "Defaule";
        int score = m_currentScore;

        WebRequest.UploadRecord(name, score);
    }

    public void FinishedUploadScore(List<MyWebRequest.GameRecord> records = null)
    {
        PlayerNameIF.transform.parent.gameObject.SetActive(false);
        if (records != null)
        {
            ShowGameScoreList(records);
        }
    }
    public void CloseUploadScore()
    {
        PlayerNameIF.transform.parent.gameObject.SetActive(false);
    }

    public void GetTop10Records()
    {
        WebRequest.GetGameTop10Records();
    }

    public void ShowGameScoreList(List<MyWebRequest.GameRecord> records)
    {
        GameScoreList.HideAllScoreItem();
        GameScoreList.transform.parent.gameObject.SetActive(true);
        for (int i = 0; i < records.Count; i++) 
        {
            GameScoreList.SetScore(i, records[i].Username, (int)records[i].Score);   
        }
    }

    public void CloseScoreList()
    {
        GameScoreList.transform.parent.gameObject.SetActive(false);
    }
}
