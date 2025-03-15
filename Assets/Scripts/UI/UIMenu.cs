using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public AudioClip menuClip;
    public AudioClip collectionsClip;

    public static void SelectPowerup(int id)
    {
        PlayerPrefs.SetInt("powerup" + id.ToString(), 1);
    }

    public static void ResetPowerups()
    {

    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OpenLeaderboard()
    {
        Application.OpenURL(API.FRONT_END_URL + "leaderboard/ava's-uprising");
    }

    public void PlayMenuSound()
    {
        var player = GameObject.Find("SoundPlayer");
        foreach (var item in player.GetComponents<SoundShifter>())
            Destroy(item);

        var shifter = player.AddComponent<SoundShifter>();
        shifter.targetClip = menuClip;
    }

    public void PlayCollectionsSound()
    {
        var player = GameObject.Find("SoundPlayer");
        foreach (var item in player.GetComponents<SoundShifter>())
            Destroy(item);

        var shifter = player.AddComponent<SoundShifter>();
        shifter.targetClip = collectionsClip;
    }
}