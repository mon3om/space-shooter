using UnityEngine;

public class UIMenu : MonoBehaviour
{
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
}