using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerupsManager : MonoBehaviour
{
    public static int SelectedPowerup = -1;

    public Button startGame;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);

        if (!TryGetComponent(out PowerupHomingMissile component)) yield break;

        GetComponent<PowerupHomingMissile>().Deactivate();
        GetComponent<PowerupShield>().Deactivate();
        GetComponent<PowerupPiercing>().Deactivate();
        GetComponent<PowerupPlusOne>().Deactivate();

        if (SelectedPowerup == 1) GetComponent<PowerupHomingMissile>().Activate();
        if (SelectedPowerup == 2) GetComponent<PowerupShield>().Activate();
        if (SelectedPowerup == 3) GetComponent<PowerupPiercing>().Activate();
        if (SelectedPowerup == 4) GetComponent<PowerupPlusOne>().Activate();
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
