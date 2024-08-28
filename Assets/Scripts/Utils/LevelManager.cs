using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_Text levelInfoText, bossAlertText;
    public List<LevelSettings> levels;

    private LevelSettings currentLevel;
    private int currentLevelIndex = 1;

    private WavesManager wavesManager;

    public static LevelManager Instance;

    public bool devMode;

    private void Awake()
    {
        Instance = this;
    }

    public float gameSpeed = 1;

    IEnumerator Start()
    {
        Time.timeScale = gameSpeed;
        yield return new WaitForEndOfFrame();
        wavesManager = GetComponent<WavesManager>();
        wavesManager.onWaveCleared += OnWaveCleared;
        wavesManager.onWaveSpawned += OnWaveSpawned;

        currentLevel = levels[0];
        UpdateUI();
        StartCoroutine(StartLevelCoroutine());
    }

    private void OnWaveSpawned(int waveId)
    {
        // Debug.Log($"Wave [{waveId}] from level [{currentLevelIndex}] just spawned");
    }

    private void UpdateUI()
    {
        levelInfoText.text = "Level: " + (currentLevelIndex + 1) +
        "\nWave: " + (wavesManager.waveId + 1) +
        "\nRemaining waves: " + (currentLevel.wavesCount - wavesManager.waveId);
        int nextBossIndex = -1;
        for (int i = currentLevelIndex; i < levels.Count; i++)
        {
            if (levels[i].bossSettings.bossPrefab != null)
            {
                nextBossIndex = i;
                break;
            }
        }

        if (nextBossIndex != -1)
            levelInfoText.text += "\nBoss in: " + (nextBossIndex - currentLevelIndex) + " levels";
        else
            levelInfoText.text += "\nGame over in: " + (levels.Count - currentLevelIndex) + " levels";
    }

    private void OnWaveCleared(WavesManager wavesManager, int waveId)
    {
        // Last wave cleared successfully
        if (wavesManager.waveId == Mathf.RoundToInt(currentLevel.wavesCount) && wavesManager.spawnedEnemies.Count == 0)
        {
            // Check if there should be a boss
            // if (currentLevelIndex > 0)
            if (currentLevelIndex % 3 == 0)
            {
                StartCoroutine(SpawnBossCoroutine());
            }
            else
            {
                LoadNextLevel();
            }
        }

        // Means a boss was generated and is successfully defeated
        // if (waveId == Mathf.RoundToInt(currentLevel.wavesCount))
        // {

        // }

        UpdateUI();
    }

    public void OnBossDefeated()
    {
        LoadNextLevel();

        var bgMusic = GameObject.FindWithTag(Tags.BACKGROUND_MUSIC);
        bgMusic.GetComponent<AudioSource>().Play();
        Destroy(GameObject.Find("temp_audio_source"));
    }

    private void LoadNextLevel()
    {
        currentLevelIndex++;
        wavesManager.levelIndex = currentLevelIndex;
        currentLevel = IncreaseDifficulty(currentLevel, currentLevel);
        StartCoroutine(StartLevelCoroutine());
        Debug.Log(currentLevelIndex);
    }

    private LevelSettings IncreaseDifficulty(LevelSettings newLevel, LevelSettings previousLevel)
    {
        var diffIncrease = newLevel.difficultySettings.difficultyIncreasePercent;

        newLevel.timeBetweenWaves = previousLevel.timeBetweenWaves * ((100 - diffIncrease) / 100);
        newLevel.difficultySettings.moderateWeight = previousLevel.difficultySettings.moderateWeight * (100 + diffIncrease) / 100;
        newLevel.difficultySettings.hardWeight = previousLevel.difficultySettings.hardWeight * (100 + diffIncrease) / 100;
        newLevel.wavesCount = previousLevel.wavesCount * ((100 + diffIncrease) / 100);

        return newLevel;
    }

    private IEnumerator StartLevelCoroutine()
    {
        if (devMode)
        {
            yield return new WaitForSeconds(1f);
            wavesManager.StartGeneratingWaves(currentLevel);

            yield break;
        }
        if (currentLevelIndex > 1)
        {
            bossAlertText.text = "LEVEL CLEARED";
            yield return new WaitForSeconds(3);
        }
        UpdateUI();
        bossAlertText.text = "LEVEL " + currentLevelIndex;
        yield return new WaitForSeconds(1.5f);
        bossAlertText.text = "START";
        yield return new WaitForSeconds(1f);
        bossAlertText.text = "";
        wavesManager.StartGeneratingWaves(currentLevel);
    }

    private IEnumerator SpawnBossCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateUI();
        var bossSettings = levels[Random.Range(1, levels.Count)].bossSettings;

        bossAlertText.text = "INCOMING BOSS";
        var bgMusic = GameObject.FindWithTag(Tags.BACKGROUND_MUSIC);
        bgMusic.GetComponent<AudioSource>().Pause();
        var previousClip = bgMusic.GetComponent<AudioSource>().clip;

        var backgroundAudioSouce = Instantiate(bgMusic).GetComponent<AudioSource>();
        backgroundAudioSouce.Stop();
        backgroundAudioSouce.name = "temp_audio_source";
        backgroundAudioSouce.volume = 1;

        backgroundAudioSouce.clip = bossSettings.alertClip;
        backgroundAudioSouce.Play();
        yield return new WaitForSeconds(6);

        bossAlertText.text = "";
        wavesManager.SpawnBoss(bossSettings.bossPrefab);

        if (bossSettings.bossClip)
        {
            backgroundAudioSouce.clip = bossSettings.bossClip;
            backgroundAudioSouce.Play();
        }
        else
        {
            backgroundAudioSouce.clip = previousClip;
            backgroundAudioSouce.Play();
        }
    }
}

[System.Serializable]
public class LevelSettings
{
    public float timeBetweenWaves = 5;
    public float wavesCount = 5;
    public LevelDifficultySettings difficultySettings;
    public BossSettings bossSettings;

    public override string ToString()
    {
        var temp = "";
        temp += "timeBetweenWaves = " + timeBetweenWaves + "\n";
        temp += "wavesCount = " + wavesCount + "\n";
        temp += "easy = " + (difficultySettings.moderateWeight + difficultySettings.hardWeight) + "%\n";
        temp += "moderate = " + difficultySettings.moderateWeight + "%\n";
        temp += "hard = " + difficultySettings.hardWeight + "%\n";

        return temp;
    }
}

[System.Serializable]
public class BossSettings
{
    public GameObject bossPrefab;
    public AudioClip alertClip;
    public AudioClip bossClip;
}

[System.Serializable]
public class LevelDifficultySettings
{
    public float moderateWeight;
    public float hardWeight;
    public float difficultyIncreasePercent = 3;
}
