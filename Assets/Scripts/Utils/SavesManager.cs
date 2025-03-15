using System.Linq;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public static System.Action OnSessionSaved;
    public static System.Action OnSessionLoaded;

    public SaveSlotData SelectedSlot;

    private AuthInfo authInfo;

    private void Start()
    {
        if (Instances.AuthInfo != null)
            StartCoroutine(API.GetGameSessions(Instances.AuthInfo.account,
                res => SlotsManager.OnSlotsLoaded?.Invoke(res),
                err => Debug.LogError(err)
            ));
    }

    private void OnEnable()
    {
        WavesManager.OnWaveCleared += SaveGameSession;
    }

    private void OnDisable()
    {
        WavesManager.OnWaveCleared -= SaveGameSession;
    }

    private void SaveGameSession(WavesManager wavesManager, int waveId, bool lastWave)
    {
        if (authInfo == null || string.IsNullOrEmpty(authInfo.account))
        {
            Debug.LogWarning("No account is connected");
            return;
        }

        SaveSlotData saveSlotData = SelectedSlot;
        saveSlotData.score = UIScoreManager.score;
        Instances.Player.TryGetComponent(out PlayerDamager pDamager);
        saveSlotData.health = (int)pDamager.health;
        saveSlotData.initHealth = (int)pDamager.initHealth;
        var powerupsByID = PowerupsManager.equippedPowerups.Select(el => el.id).ToArray();
        saveSlotData.powerups = string.Join(",", powerupsByID);
        saveSlotData.level = Instances.LevelManager.currentLevelIndex;
        saveSlotData.wave = waveId;
        saveSlotData.account = authInfo.account;
        if (lastWave)
        {
            saveSlotData.level++;
            saveSlotData.wave = 0;
        }

        StartCoroutine(API.SaveGameSession(saveSlotData,
        res =>
        {
            SelectedSlot._id = res._id;
        },
        err =>
        {
            Debug.LogError(err);
        }));
    }
}