using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RawPowerup : MonoBehaviour
{
    [SerializeField] private PowerupUIManager powerupUIManager;

    private PowerupScriptableObject selectedPowerup;
    private System.Func<PowerupScriptableObject> GetRandomPowerup;

    public void SetSelectedPowerup(PowerupScriptableObject powerupScriptableObject, System.Func<PowerupScriptableObject> getRandomPowerup)
    {
        selectedPowerup = powerupScriptableObject;
        powerupUIManager.SetData(selectedPowerup);
        GetRandomPowerup = getRandomPowerup;
        AttachPowerupBaseComponent();
    }

    private void AttachPowerupBaseComponent()
    {
        var rawScript = selectedPowerup.GetPowerupType();
        var powerupBase = gameObject.AddComponent(rawScript) as PowerupBase;
        powerupBase.ReplacePowerup = ReplacePowerup;
        powerupBase.powerupScriptableObject = selectedPowerup;
    }

    private void ReplacePowerup()
    {
        // Clean previous added PowerupBase scripts 
        // Only if the powerup was picked and needs to be altered
        foreach (var item in transform.GetComponentsInChildren<PowerupBase>())
        {
            Debug.Log($"Destroying previous attached PowerupBase script [{item.name}] as it can't be equipped");
            Destroy(item);
        }

        var randomPowerup = GetRandomPowerup();
        SetSelectedPowerup(randomPowerup, GetRandomPowerup);
    }

    public void ActivatePowerup(PowerupScriptableObject powerup)
    {
        var rawScript = powerup.GetPowerupType();
        var powerupBase = gameObject.AddComponent(rawScript) as PowerupBase;
        powerupBase.powerupScriptableObject = powerup;
        powerupBase.Activate();
    }

    // private List<PowerupScriptableObject> allPowerups;

    // private static List<PowerupScriptableObject> AllPowerupsStatic = new();
    // public static List<PowerupScriptableObject> onScreenPowerups = new();

    // private PowerupBase powerupScript;

    // public System.Action<PowerupScriptableObject, GameObject> ReplacePowerup;

    // [Space]
    // [Header("Debug")]
    // public int manualSelectedID = -1;

    // private void Start()
    // {
    //     if (manualSelectedID != -1)
    //         PickRandomPowerup(new(), manualSelectedID);
    // }

    // public PowerupScriptableObject PickRandomPowerup(List<PowerupScriptableObject> filter, int manualId = -1)
    // {
    //     allPowerups = LoadAllPowerups();
    //     allPowerups = allPowerups.Where(el => el.available).ToList();
    //     var filteredPowerups = allPowerups.Where(el => filter.Any(el1 => el1.id == el.id) == false).ToList();

    //     if (allPowerups.Count == 0)
    //         throw new System.Exception("Powerups count is 0 after filtering the whole list of size " + allPowerups.Count + " with a filter of size " + filter.Count + "");

    //     int selected = Random.Range(0, filteredPowerups.Count);
    //     selectedPowerup = filteredPowerups[selected];

    //     if (manualId != -1)
    //         selectedPowerup = filteredPowerups.First(el => el.id == manualId);

    //     // Clean previous added PowerupBase scripts 
    //     // Only if the powerup was picked and needs to be altered
    //     foreach (var item in transform.GetComponentsInChildren<PowerupBase>())
    //     {
    //         if (!item) continue;
    //         Debug.Log($"Destroying previous attached PowerupBase script [{item.name}] as it can't be equipped");
    //         Destroy(item);
    //     }

    //     // Assigning respective PowerBase script for Acivation
    //     var rawScript = selectedPowerup.GetPowerupType();
    //     powerupScript = gameObject.AddComponent(rawScript) as PowerupBase;
    //     powerupScript.ReplacePowerup = ReplacePowerup;
    //     powerupScript.powerupScriptableObject = selectedPowerup;

    //     return selectedPowerup;
    // }

    // public void SetPowerup(PowerupScriptableObject powerup)
    // {
    //     selectedPowerup = powerup;
    // }





    // public void ActivatePowerup()
    // {
    //     powerupScript.Activate(selectedPowerup);
    // }



    // private List<PowerupScriptableObject> LoadAllPowerups()
    // {
    //     if (AllPowerupsStatic.Count == 0)
    //         AllPowerupsStatic = Resources.LoadAll<PowerupScriptableObject>("ScriptableObjects/Powerups").ToList();

    //     return AllPowerupsStatic;
    // }
}
