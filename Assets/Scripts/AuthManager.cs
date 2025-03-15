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
    public string notConnectedText = "Please login in the launcher and reopen the game from there";

    [DllImport("__Internal")]
    private static extern void SaveAndCleanURL();
    [DllImport("__Internal")]
    private static extern void ClearAccountAndUsername();
    [DllImport("__Internal")]
    private static extern string GetAccountAndUsername();

    private AuthInfo authInfo;

    private string imgUrl = "https://img.freepik.com/premium-photo/cyber-security-ransomware-email-phishing-encrypted-technology-digital-information-protected-secured_951586-20618.jpg";


    void Start()
    {
        logoutBtn.onClick.AddListener(Logout);
        ExtractDataFromUrlAndLoad();
        // #if UNITY_EDITOR
        //         Debug.Log("Downloading image...");
        //         StartCoroutine(DownloadImage(imgUrl, tex => Instances.profileTexture = tex, err => { }));
        // #endif
    }

    private void OnEnable() => API.OnImageDownloaded += OnImageDownloaded;
    private void OnDisable() => API.OnImageDownloaded -= OnImageDownloaded;

    private void ExtractDataFromUrlAndLoad()
    {
#if UNITY_WEBGL
        if (Application.isEditor) return;
        SaveAndCleanURL();
        var rawData = GetAccountAndUsername();
        if (!string.IsNullOrEmpty(rawData))
        {
            authInfo = JsonUtility.FromJson<AuthInfo>(rawData);
            authText.DOText(authInfo.username + " - " + authInfo.account, 1);
            logoutBtn.gameObject.SetActive(true);
            StartCoroutine(API.DownloadImage(authInfo.profilePicture));

            Instances.AuthInfo = authInfo;
        }
        else
        {
            logoutBtn.gameObject.SetActive(false);
            authText.DOText(notConnectedText, 1);
        }
#endif
    }

    public void Logout()
    {
#if UNITY_WEBGL
        if (Application.isEditor) return;
        Application.OpenURL(API.FRONT_END_URL + "logout");
        ClearAccountAndUsername();
        logoutBtn.gameObject.SetActive(false);
        authText.DOText(notConnectedText, 1);
#endif
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus)
            ExtractDataFromUrlAndLoad();
    }

    private void OnImageDownloaded(Texture2D texture2D)
    {
        if (texture2D != null)
        {
            authInfo.profileTexture = texture2D;
            Instances.AuthInfo = authInfo;
        }
    }
}

public class AuthInfo
{
    public bool IsConnected { get { return !string.IsNullOrEmpty(account); } private set { } }

    public string username;
    public string account;
    public int score;
    public int bestScore;
    public string profilePicture;

    public Texture2D profileTexture;

    public AuthInfo()
    {
#if UNITY_EDITOR
        account = "testaccount";
        username = "testusername";
#endif
    }

    public IEnumerator SaveScoreCoroutine(int _score, System.Action<SaveScoreData> onSuccess, System.Action<SaveScoreData> onError)
    {
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
