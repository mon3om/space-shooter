using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIPowerupsManager : MonoBehaviour
{
    public Transform powerupsContainer;
    public GameObject uIpowerupPrefab;


    private void Start()
    {
        LoadAllPowerups();
        var powerups = PowerupsManager.AllPowerups;
        powerups.Sort((x, y) => x.itemName.CompareTo(y.itemName));
        foreach (var item in powerups)
        {
            if (!item.available) continue;

            var pu = Instantiate(uIpowerupPrefab, powerupsContainer);
            UIPowerupItem puItem = pu.GetComponent<UIPowerupItem>();
            puItem.powerupScriptableObject = item;
        }
    }

    private List<PowerupScriptableObject> LoadAllPowerups()
    {
        if (PowerupsManager.AllPowerups.Count == 0)
            PowerupsManager.AllPowerups = Resources.LoadAll<PowerupScriptableObject>("ScriptableObjects/Powerups").ToList();

        return PowerupsManager.AllPowerups;
    }
}
