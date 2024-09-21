using System.Collections;
using System.Runtime.InteropServices;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public TextMeshProUGUI authText;
    public Button logoutBtn;

    [DllImport("__Internal")]
    private static extern void SaveAndCleanURL();
    [DllImport("__Internal")]
    private static extern void ClearAccountAndUsername();
    [DllImport("__Internal")]
    private static extern string GetAccountAndUsername();

    void Start()
    {
        logoutBtn.onClick.AddListener(Logout);
        ExtractDataFromUrlAndLoad();
    }

    private void ExtractDataFromUrlAndLoad()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveAndCleanURL();
        var rawData = GetAccountAndUsername();
        if (!string.IsNullOrEmpty(rawData))
        {
            AuthInfo.Instance = JsonUtility.FromJson<AuthInfo>(rawData);
            authText.DOText(AuthInfo.Instance.username + " - " + AuthInfo.Instance.account, 1);
            logoutBtn.gameObject.SetActive(true);
        }else{
            logoutBtn.gameObject.SetActive(false);
            authText.DOText("No user found, please log in in the launcher and reopen the game from there.", 1);
        }
        Debug.Log("Username = " + AuthInfo.Instance.username);
        Debug.Log("Account = " + AuthInfo.Instance.account);
#endif
    }

    public void Logout()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.OpenURL("http://localhost:5173/logout");
        ClearAccountAndUsername();
        logoutBtn.gameObject.SetActive(false);
        authText.text = "";
#endif
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
        {
            ExtractDataFromUrlAndLoad();
        }
    }
}

public class AuthInfo
{
    public static AuthInfo Instance = new();
    public bool IsConnected { get { return !string.IsNullOrEmpty(account); } private set { } }

    public string username;
    public string account;
    public int score;
    public int bestScore;

    public IEnumerator SaveScoreCoroutine(int _score, System.Action<SaveScoreData> onSuccess, System.Action<SaveScoreData> onError)
    {
#if UNITY_EDITOR
        account = "testaccount";
        username = "testusername";
#endif

        if (!IsConnected)
        {
            Debug.LogWarning("User not connected, the score won't be saved");
            yield break;
        }


        SaveScoreData saveScoreData = new()
        {
            score = _score,
            account = account,
            username = username,
        };

        yield return API.SaveScore(saveScoreData, onSuccess, onError);
    }

    public void Logout()
    {
        username = "";
        account = "";
        score = 0;
    }
}
