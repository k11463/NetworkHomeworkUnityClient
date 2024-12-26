using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class MyWebRequest : MonoBehaviour
{
    private string hostAddress = "http://192.168.31.200:8080/";
    public class GameRecord
    { 
        public string Username;
        public uint Score;
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    //IEnumerator GetHighestRecord()
    //{
    //    // set the request URL
    //    string url = hostAddress + "Record";

    //    // using UnityWebRequest and Get for send web request
    //    UnityWebRequest www = UnityWebRequest.Get(url);

    //    // wait for response
    //    yield return 0;

    //    // check be successful

    //    // log the JSON string in result

    //    // JSON string to object
    //}

    public void GetGameTop10Records()
    {
        StartCoroutine(GetTop10Records());
    }

    IEnumerator GetTop10Records()
    {
        string url = hostAddress + "Records";
        
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
        {
            string res = www.downloadHandler.text;
            var records = JsonConvert.DeserializeObject<List<GameRecord>>(res);
            GameManager.Instance.ShowGameScoreList(records);
        }
    }

    public void UploadRecord(string name, int score)
    {
        GameRecord record = new GameRecord
        {
            Username = name,
            Score = (uint)score,
        };

        string jStr = JsonConvert.SerializeObject(record);

        StartCoroutine(UploadRecord(jStr));
    }

    IEnumerator UploadRecord(string postData)
    {
        string url = hostAddress + "Record";

        UnityWebRequest www = UnityWebRequest.Post(url, postData, "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
        {
            string res = www.downloadHandler.text;
            var records = JsonConvert.DeserializeObject<List<GameRecord>>(res);
            GameManager.Instance.FinishedUploadScore(records);
        }
    }
}
