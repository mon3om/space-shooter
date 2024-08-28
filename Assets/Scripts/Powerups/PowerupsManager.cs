using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupsManager : MonoBehaviour
{
    public static int SelectedPowerup = -1;
    public static List<PowerupScriptableObject> equippedPowerups = new();
    public PowerupScriptableObject defaultBulletPowerup;

    public Button startGame;

    public static int piercingCount = 0;
    public static int bounceCount = 0;
    public static int shotsCount = 1;
    public static int playerHealth = 5;
    public static ShootingSettings playerShootingSettings;

    private void Start()
    {
        if (equippedPowerups.Count == 0 && defaultBulletPowerup != null)
        {
            equippedPowerups = new()
            {
                defaultBulletPowerup
            };
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

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
}
