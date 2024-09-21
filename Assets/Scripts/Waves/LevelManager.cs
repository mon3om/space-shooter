using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int levelsBeforeBoss = 3;
    public List<LevelSettings> levels;

    [Space]
    public bool devMode;
    public float gameSpeed = 1;

    private LevelSettings currentLevel;
    private int currentLevelIndex = 1;
    private bool isBossSpawned = false;
    // Components
    private WavesUIManager wavesUIManager;
    private WavesManager wavesManager;

    private void Awake()
    {
        Instances.LevelManager = this;
    }

    IEnumerator Start()
    {
        Time.timeScale = gameSpeed;
        if (gameSpeed != 1)
            Debug.LogWarning("GameSpeed is " + gameSpeed);

        yield return new WaitForEndOfFrame();
        wavesUIManager = GetComponent<WavesUIManager>();
        wavesManager = GetComponent<WavesManager>();
        wavesManager.onWaveCleared += OnWaveCleared;
        wavesManager.onWaveSpawned += OnWaveSpawned;

        currentLevel = levels[0];
        StartCoroutine(StartLevelCoroutine());
    }

    private void OnWaveSpawned(int waveId)
    {
        // Debug.Log($"Wave [{waveId}] from level [{currentLevelIndex}] just spawned");
    }

    private void OnWaveCleared(WavesManager wavesManager, int waveId)
    {
        bool lastWaveInLevel = false;
        if (wavesManager.waveId == currentLevel.WavesCount && wavesManager.spawnedEnemies.Count == 0)
            lastWaveInLevel = true;

        if (lastWaveInLevel)
        {
            if (currentLevelIndex % levelsBeforeBoss == 0 || devMode)
                StartCoroutine(SpawnBossCoroutine());
            else
                LoadNextLevel();
        }
    }

    public void OnBossDefeated()
    {
        if (!isBossSpawned) return;
        isBossSpawned = false;

        LoadNextLevel();

        var bgMusic = GameObject.FindWithTag(Tags.BACKGROUND_MUSIC);
        bgMusic.GetComponent<AudioSource>().Play();
        if (tempAudioSource)
        {
            Destroy(tempAudioSource.gameObject);
            tempAudioSource = null;
        }
    }

    private void LoadNextLevel()
    {
        currentLevelIndex++;
        wavesManager.levelIndex = currentLevelIndex;
        currentLevel = currentLevel.IncreaseDifficulty(currentLevel, currentLevel);
        StartCoroutine(StartLevelCoroutine());
    }

    private IEnumerator StartLevelCoroutine()
    {
        if (devMode)
        {
            yield return new WaitForSeconds(1f);
            wavesManager.StartGeneratingWaves(currentLevel);
            yield break;
        }

        yield return wavesUIManager.UpdateUI(currentLevelIndex);
        wavesManager.StartGeneratingWaves(currentLevel);
    }

    private AudioSource tempAudioSource;
    private IEnumerator SpawnBossCoroutine()
    {
        var bossSettings = levels[Random.Range(1, levels.Count)].bossSettings;

        wavesUIManager.bossAlertText.DOText("INCOMING BOSS", 1);
        var bgMusic = GameObject.FindWithTag(Tags.BACKGROUND_MUSIC);
        bgMusic.GetComponent<AudioSource>().Pause();
        var previousClip = bgMusic.GetComponent<AudioSource>().clip;

        tempAudioSource = Instantiate(bgMusic).GetComponent<AudioSource>();
        tempAudioSource.Stop();
        tempAudioSource.name = "temp_audio_source";
        tempAudioSource.volume = 1;

        tempAudioSource.clip = bossSettings.alertClip;
        tempAudioSource.Play();
        Instances.CameraShaker.StartBossGlitching(6, tempAudioSource);
        yield return new WaitForSeconds(6);
        Instances.CameraShaker.StopBossGlitching();
        wavesUIManager.bossAlertText.text = "";
        wavesManager.SpawnBoss(bossSettings.bossPrefab);
        isBossSpawned = true;

        if (bossSettings.bossClip)
        {
            tempAudioSource.clip = bossSettings.bossClip;
            tempAudioSource.Play();
        }
        else
        {
            tempAudioSource.clip = previousClip;
            tempAudioSource.Play();
        }
    }
}

