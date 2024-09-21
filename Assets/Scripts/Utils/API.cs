using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour
{
    public string apiUrl;
    public string frontEndUrl;
    public static string API_URL { private set; get; } = "http://localhost:5000/";
    public static string FRONT_END_URL { private set; get; } = "http://localhost:5173/";

    private static API Instance = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(apiUrl))
            API_URL = apiUrl;
        if (!string.IsNullOrEmpty(frontEndUrl))
            FRONT_END_URL = frontEndUrl;
    }

    public static IEnumerator SaveScore(SaveScoreData playerScoreData, Action<SaveScoreData> onSuccess, Action<SaveScoreData> onError)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(API_URL + "players", JsonUtility.ToJson(playerScoreData), "application/json");
        yield return webRequest.SendWebRequest();

        SaveScoreData tsxData = JsonUtility.FromJson<SaveScoreData>(webRequest.downloadHandler.text);
        Debug.Log(webRequest.downloadHandler.text);
        if (webRequest.result == UnityWebRequest.Result.Success)
            onSuccess(tsxData);
        else
            onError(tsxData);
    }
}

public class SaveScoreData
{
    public string account;
    public string username;
    public int score;
    public bool newHighScore;
    public string game = "1";
    public string error;
}
