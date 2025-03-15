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
    public bool dev;

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
        if (!dev)
        {
            if (!string.IsNullOrEmpty(apiUrl))
                API_URL = apiUrl;
            if (!string.IsNullOrEmpty(frontEndUrl))
                FRONT_END_URL = frontEndUrl;
        }
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

    public static IEnumerator SaveGameSession(SaveSlotData gameSessionData, Action<SaveSlotData> onSuccess, Action<SaveSlotData> onError)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(API_URL + "sessions", JsonUtility.ToJson(gameSessionData), "application/json");
        yield return webRequest.SendWebRequest();

        SaveSlotData tsxData = JsonUtility.FromJson<SaveSlotData>(webRequest.downloadHandler.text);
        if (webRequest.result == UnityWebRequest.Result.Success)
            onSuccess(tsxData);
        else
            onError(tsxData);
    }

    public static IEnumerator GetGameSessions(string account, Action<SaveSlotData[]> onSuccess, Action<string> onError)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(API_URL + "sessions/all/" + account);
        yield return webRequest.SendWebRequest();

        string json = JsonHelper.FixJson(webRequest.downloadHandler.text);
        var res = JsonHelper.FromJson<SaveSlotData>(json);
        if (webRequest.result == UnityWebRequest.Result.Success)
            onSuccess(res);
        else
            onError(webRequest.downloadHandler.text);
    }

    public static Action<Texture2D> OnImageDownloaded;
    public static IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            void OnError(string error)
            {
                Debug.LogError(error);
            }

            OnError(request.error);
        }
        else
        {
            OnImageDownloaded?.Invoke(((DownloadHandlerTexture)request.downloadHandler).texture);
        }
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

[Serializable]
public class SaveSlotData
{
    public string _id;
    public string account;
    public int slotIndex;
    public int score;
    public string game = "1";
    public int level = 0;
    public int wave = 0;
    public string powerups = "";
    public int health = 0;
    public int initHealth = 0;
    public string createdAt;
    public string updatedAt;
}

public static class JsonHelper
{
    public static string FixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}