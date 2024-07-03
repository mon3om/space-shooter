using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_Text levelInfoText, bossAlertText;
    public List<LevelSettings> levels;

    private LevelSettings currentLevel;
    private int currentLevelIndex = 0;

    private WavesManager wavesManager;


    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        wavesManager = GetComponent<WavesManager>();
        wavesManager.onWaveCleared += OnWaveCleared;
        wavesManager.onWaveSpawned += OnWaveSpawned;

        currentLevel = levels[currentLevelIndex];
        UpdateUI();
        StartCoroutine(StartLevelCoroutine());
    }

    private void OnWaveSpawned(int waveId)
    {
        Debug.Log($"Wave [{waveId}] from level [{currentLevelIndex}] just spawned");
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
        Debug.Log(wavesManager.waveId + " == total = " + Mathf.RoundToInt(currentLevel.wavesCount));
        // Last wave cleared successfully
        if (wavesManager.waveId == Mathf.RoundToInt(currentLevel.wavesCount) && wavesManager.spawnedEnemies.Count == 0)
        {
            // Check if there should be a mini boss
            if (currentLevel.bossSettings.bossPrefab != null)
            {
                StartCoroutine(SpawnBossCoroutine());
            }
            else
            {
                LoadNextLevel();
            }
        }

        // Means a boss was generated and is successfully defeated
        if (waveId == Mathf.RoundToInt(currentLevel.wavesCount))
        {
            LoadNextLevel();

            var bgMusic = GameObject.FindWithTag(Tags.BACKGROUND_MUSIC);
            bgMusic.GetComponent<AudioSource>().Play();
            Destroy(GameObject.Find("temp_audio_source"));
        }

        UpdateUI();
    }

    private void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
        {
            bossAlertText.text = "GAME BEATEN";
            Debug.Log("Congrats, game beaten");
            return;
        }
        currentLevel = IncreaseDifficulty(levels[currentLevelIndex], currentLevel);

        StartCoroutine(StartLevelCoroutine());
    }

    private LevelSettings IncreaseDifficulty(LevelSettings newLevel, LevelSettings previousLevel)
    {
        newLevel.timeBetweenWaves = previousLevel.timeBetweenWaves * ((100 - newLevel.difficultySettings.difficultyIncreasePercent) / 100);
        newLevel.difficultySettings.moderateWeight = previousLevel.difficultySettings.moderateWeight * (100 + newLevel.difficultySettings.difficultyIncreasePercent) / 100;
        newLevel.difficultySettings.hardWeight = previousLevel.difficultySettings.hardWeight * (100 + newLevel.difficultySettings.difficultyIncreasePercent) / 100;
        newLevel.wavesCount = previousLevel.wavesCount * ((100 + newLevel.difficultySettings.difficultyIncreasePercent) / 100);

        return newLevel;
    }

    private IEnumerator StartLevelCoroutine()
    {
        if (currentLevelIndex > 0)
        {
            bossAlertText.text = "LEVEL CLEARED";
            yield return new WaitForSeconds(5);
        }
        UpdateUI();
        bossAlertText.text = "LEVEL " + (currentLevelIndex + 1);
        yield return new WaitForSeconds(2);
        bossAlertText.text = "START";
        yield return new WaitForSeconds(1f);
        bossAlertText.text = "";
        wavesManager.StartGeneratingWaves(currentLevel);
    }

    private IEnumerator SpawnBossCoroutine()
    {
        UpdateUI();
        bossAlertText.text = "INCOMING BOSS";
        var bgMusic = GameObject.FindWithTag(Tags.BACKGROUND_MUSIC);
        bgMusic.GetComponent<AudioSource>().Pause();
        AudioClip prevClip = bgMusic.GetComponent<AudioSource>().clip;

        var backgroundAudioSouce = Instantiate(bgMusic).GetComponent<AudioSource>();
        backgroundAudioSouce.name = "temp_audio_source";
        backgroundAudioSouce.volume = 1;
        if (currentLevel.bossSettings.alertClip)
        {
            backgroundAudioSouce.clip = currentLevel.bossSettings.alertClip;
            backgroundAudioSouce.Play();
            yield return new WaitForSeconds(10);
        }

        bossAlertText.text = "";
        wavesManager.SpawnBoss(currentLevel.bossSettings.bossPrefab);

        if (currentLevel.bossSettings.bossClip)
        {
            backgroundAudioSouce.clip = currentLevel.bossSettings.bossClip;
            backgroundAudioSouce.Play();
        }
        else
        {
            backgroundAudioSouce.clip = prevClip;
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
