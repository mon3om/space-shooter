using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        // Equip persistent powerups
        if (_InspectorPreEquippedPowerups.Count > 0 || PreEquippedPowerups.Count > 0)
        {
            foreach (var item in _InspectorPreEquippedPowerups)
                if (!PreEquippedPowerups.Contains(item))
                    PreEquippedPowerups.Add(item);

            var droppablePowerup = gameObject.AddComponent<RawPowerup>();
            foreach (var item in PreEquippedPowerups)
            {
                equippedPowerups.Add(item);
                droppablePowerup.ActivatePowerup(item);
            }
            Destroy(droppablePowerup);
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
