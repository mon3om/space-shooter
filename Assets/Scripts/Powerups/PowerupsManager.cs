using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
TODOs:
    - Add smoke when ship damaged
    - Add some powerups
        - explosive bullets
    - Implement powerups manager for spawning powerups after defeating bosses
    - Add special enemy for droping powerups
    - Add 2 bosses
    - Finish 2 impcomplete enemies
        - Star shap shooter
        - Laser enemy
    
*/

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
