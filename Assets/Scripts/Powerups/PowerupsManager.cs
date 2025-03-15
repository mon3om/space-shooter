using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerupsManager : MonoBehaviour
{
    public static int SelectedPowerup = -1;
    public static List<PowerupScriptableObject> AllPowerups = new();
    public static List<PowerupScriptableObject> equippedPowerups = new();

    [SerializeField] private List<PowerupScriptableObject> _InspectorPreEquippedPowerups = new();
    public static List<PowerupScriptableObject> PreEquippedPowerups = new();

    public static int piercingCount = 0;
    public static int bounceCount = 0;
    public static int shotsCount = 1;
    public static float powerCoreModifier = 1;
    public static int bossPowerupsCount = 4;
    public static ShootingSettings playerShootingSettings;

    public static PowerupsManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Application.targetFrameRate = 60;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) return;
        StartCoroutine(OnGameStartedCoroutine());
    }

    private IEnumerator OnGameStartedCoroutine()
    {
        yield return new WaitForEndOfFrame();

        powerCoreModifier = 1;
        bossPowerupsCount = 4;


        // Equipe from saved slots
        if (equippedPowerups.Count > 0)
        {
            var rawPowerupComponent = gameObject.AddComponent<RawPowerup>();
            for (int i = 0; i < equippedPowerups.Count; i++)
                rawPowerupComponent.ActivatePowerup(equippedPowerups[i]);

            Destroy(rawPowerupComponent);
        }

        // Equip persistent powerups
        if (_InspectorPreEquippedPowerups.Count > 0 || PreEquippedPowerups.Count > 0)
        {
            if (equippedPowerups.Count == 0)
                foreach (var item in _InspectorPreEquippedPowerups)
                    if (!PreEquippedPowerups.Contains(item))
                        PreEquippedPowerups.Add(item);

            var rawPowerupComponent = gameObject.AddComponent<RawPowerup>();
            foreach (var item in PreEquippedPowerups)
            {
                equippedPowerups.Add(item);
                rawPowerupComponent.ActivatePowerup(item);
            }
            Destroy(rawPowerupComponent);
        }

    }

    public void EquipPowerupsByString(string powerupsStringCommaSeparated)
    {
        if (string.IsNullOrEmpty(powerupsStringCommaSeparated))
        {
            Debug.LogWarning("Empty string");
            return;
        }
        string[] strings = powerupsStringCommaSeparated.Split(",");
        equippedPowerups.Clear();
        foreach (var item in strings)
        {
            if (string.IsNullOrEmpty(item.Trim())) continue;
            var po = AllPowerups.Find(el => el.id == int.Parse(item.Trim()));
            equippedPowerups.Add(po);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
