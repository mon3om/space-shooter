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

    private LevelSettings level;
    [HideInInspector] public int currentLevelIndex = 1;
    private bool isBossSpawned = false;
    // Components
    private WavesUIManager wavesUIManager;
    private WavesManager wavesManager;

    private void Awake()
    {
        Instances.LevelManager = this;
    }

    private StoryScriptableObject[] stories;
    private int storyIndex = 0;

    IEnumerator Start()
    {
        stories = Resources.LoadAll<StoryScriptableObject>("ScriptableObjects/Story");
        Time.timeScale = gameSpeed;
        if (gameSpeed != 1)
            Debug.LogWarning("GameSpeed is " + gameSpeed);

        yield return new WaitForEndOfFrame();
        wavesUIManager = GetComponent<WavesUIManager>();
        wavesManager = GetComponent<WavesManager>();
        WavesManager.OnWaveCleared += OnWaveCleared;
        WavesManager.OnWaveSpawned += OnWaveSpawned;

        level = levels[0];
        level = level.CalculateDifficulty(levels[0], currentLevelIndex);

        if (currentLevelIndex == 1 && wavesManager.waveId == 0)
        {
            Instances.StoryManager.StartStoryTelling(() =>
                {
                    StartCoroutine(StartLevelCoroutine());
                    storyIndex++;
                }, stories[storyIndex]);
        }
        else
        {
            StartCoroutine(StartLevelCoroutine());
        }
    }

    public void SetLevelFromSavedData(SaveSlotData saveSlotData)
    {
        if (level == null)
            level = levels[0];

        currentLevelIndex = saveSlotData.level;
        level = level.CalculateDifficulty(levels[0], saveSlotData.level);
    }

    private void OnWaveSpawned(int waveId)
    {
        // Debug.Log($"Wave [{waveId}] from level [{currentLevelIndex}] just spawned");
    }

    private void OnWaveCleared(WavesManager wavesManager, int waveId, bool lastWave)
    {
        if (lastWave)
        {
            if (currentLevelIndex % levelsBeforeBoss == 0 || devMode)
            {
                if (!isBossSpawned)
                {
                    StartCoroutine(SpawnBossCoroutine());
                    isBossSpawned = true;
                }
            }
            else
            {
                LoadNextLevel();
            }
        }
    }

    public void OnBossDefeated()
    {
        if (!isBossSpawned) return;
        isBossSpawned = false;

        if (storyIndex < stories.Length)
        {
            Instances.StoryManager.StartStoryTelling(() =>
                {
                    LoadNextLevel();
                    storyIndex++;
                },
            stories[storyIndex]);
        }
        else
        {
            LoadNextLevel();
        }

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
        level = level.CalculateDifficulty(levels[0], currentLevelIndex);

        StartCoroutine(StartLevelCoroutine());
    }

    private IEnumerator StartLevelCoroutine()
    {
        if (devMode)
        {
            yield return new WaitForSeconds(1f);
            wavesManager.StartGeneratingWaves(level);
            yield break;
        }

        yield return wavesUIManager.UpdateUI(currentLevelIndex);
        wavesManager.StartGeneratingWaves(level);
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
        // Instances.CameraShaker.StopBossGlitching();
        wavesUIManager.bossAlertText.text = "";
        wavesManager.SpawnBoss(bossSettings.bossPrefab);

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

