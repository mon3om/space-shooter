using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerupDropperPrefab : MonoBehaviour
{
    public GameObject rawPowerupPrefab;
    public int powerupsCount = 1;
    public List<int> powerupsIdForDebug;

    private static List<PowerupScriptableObject> AllPowerups = new();
    private List<PowerupScriptableObject> pickedPowerups = new();

    private int nonRandomId = -1;

    private void Start()
    {
        InstantiatePowerups();
    }

    private void InstantiatePowerups()
    {
        int count = powerupsCount;
        if (powerupsIdForDebug.Count > 0)
        {
            Debug.LogWarning("Manual powerups are forced");
            count = powerupsIdForDebug.Count;
        }

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(rawPowerupPrefab, transform.position, Quaternion.identity, transform);
            var position = GetPowerupPosition(count, i);
            Debug.Log(position);
            go.GetComponent<DirectionalMover>().MoveTowardsPoint(position);

            // For debug: Sets the current manual ID
            if (powerupsIdForDebug.Count > 0)
                nonRandomId = powerupsIdForDebug[i];
            //

            var selectedPowerup = PickPowerup();
            go.GetComponent<RawPowerup>().SetSelectedPowerup(selectedPowerup, PickPowerup);
        }
    }

    private PowerupScriptableObject PickPowerup()
    {
        var powerups = GetFilteredPowerups();
        PowerupScriptableObject pickedPowerup;

        if (nonRandomId == -1)
        {
            pickedPowerup = powerups[Random.Range(0, powerups.Count)];
        }
        else // For debug
        {
            Debug.Log("Getting debug powerup with ID = " + nonRandomId);
            pickedPowerup = powerups.First(el => el.id == nonRandomId);
            nonRandomId = -1;
        }

        pickedPowerups.Add(pickedPowerup);
        return pickedPowerup;
    }

    private List<PowerupScriptableObject> GetFilteredPowerups()
    {
        LoadAllPowerups();
        // Filter based on availability
        var availablePowerups = AllPowerups.Where(x => x.available).ToList();

        // Filter based on picked powerups
        var filteredPowerups = availablePowerups.Where(x => !pickedPowerups.Contains(x)).ToList();

        // Filter based on required powerups
        var impossiblePowerups = availablePowerups.Where(el => el.requiredPowerups.Length > 0).ToList();
        impossiblePowerups = impossiblePowerups.Where(el =>
        {
            // Check if the current element's required powerups are all in PowerupsManager.equippedPowerups 
            bool possible = el.requiredPowerups.Any(x => PowerupsManager.equippedPowerups.Contains(x));
            return !possible;
        }).ToList();
        filteredPowerups = filteredPowerups.Where(x => !impossiblePowerups.Contains(x)).ToList();

        if (filteredPowerups.Count < powerupsCount)
            Debug.LogError("Avaialble filtered powerps count is insufficient!");

        return filteredPowerups;
    }

    private Vector2 GetPowerupPosition(int count, int index)
    {
        float angleStep = 360f / count;
        float angle = index * angleStep;
        float radians = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(radians) * Mathf.Clamp((count - 1) * 0.5f, 0, 2.5f);
        float y = Mathf.Sin(radians) * Mathf.Clamp((count - 1) * 0.5f, 0, 2.5f);
        return new(x, y);
    }

    private void LoadAllPowerups()
    {
        if (AllPowerups.Count == 0)
            AllPowerups = Resources.LoadAll<PowerupScriptableObject>("ScriptableObjects/Powerups").ToList();
    }
}