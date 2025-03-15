using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wave.Helper;


public class WavesManager : MonoBehaviour
{
    [HideInInspector] public LevelSettings levelSettings;
    public int waveId = 0;

    [HideInInspector] public List<EnemyAIBase> spawnedEnemies = new();

    private List<string> lastSpawnedEnemies = new();
    private List<string> lastSpawnedDifficulty = new();

    [HideInInspector] public int levelIndex = 0;

    // Events
    public static System.Action<WavesManager, int, bool> OnWaveCleared; // <this, waveId, lastWave>
    public static System.Action<int> OnWaveSpawned; // <waveId>
    public static System.Action OnMiniBossDefeated;

    private bool powerupDropperSpawned = false;

    // Components
    private EnemyFactory enemyFactory;
    private WaveEnemyPicker waveEnemyPicker;

    private void Awake()
    {
        Instances.WavesManager = this;
    }

    private void Start()
    {
        enemyFactory = GetComponentInChildren<EnemyFactory>();
        waveEnemyPicker = GetComponent<WaveEnemyPicker>();
    }

    private IEnumerator SpawnCoroutine()
    {
        var pickedEnemy = waveEnemyPicker.GetSingleEnemyDifficultyBased(levelSettings, lastSpawnedEnemies);
        SpawnPowerupDropper();
        Spawn(pickedEnemy);

        if (waveId == levelSettings.WavesCount)
            yield break;

        yield return new WaitForSeconds(levelSettings.timeBetweenWaves);
        StartCoroutine(SpawnCoroutine());
    }

    [Space]
    public GameObject temp_powerupDropper;
    private void SpawnPowerupDropper()
    {
        if (powerupDropperSpawned) return;

        if ((levelIndex - 2) % 3 == 0 && waveId > levelSettings.WavesCount / 2f)
        {
            WaveEnemy waveEnemy = new()
            {
                enemyPrefab = temp_powerupDropper,
                waveEnteringPositions = new WaveEnteringPosition[] { WaveEnteringPosition.ShouldEnterFromTop },
                count = new IntRange(1, 1, 1)
            };
            Spawn(waveEnemy);
            powerupDropperSpawned = true;
        }
    }

    private void Spawn(WaveEnemy pickedEnemy)
    {
        var spawned = enemyFactory.Spawn(pickedEnemy, waveId, OnEnemyDestroy);
        PositionManager.SetPositions(spawned, pickedEnemy);

        foreach (var item in spawned)
            spawnedEnemies.Add(item.GetComponent<EnemyAIBase>());
        OnWaveSpawned?.Invoke(waveId);
        waveId++;
    }

    private void OnEnemyDestroy(EnemyAIBase enemyAIBase, int waveId)
    {
        spawnedEnemies.Remove(enemyAIBase);

        // If wave is cleared
        if (spawnedEnemies.Where(_ => _.enemyIdentifier.waveId == waveId).Count() == 0)
        {
            bool lastWaveInLevel = false;
            if (this.waveId == levelSettings.WavesCount && spawnedEnemies.Count == 0)
                lastWaveInLevel = true;

            if (!enemyAIBase.name.ToLower().Contains("boss"))
                OnWaveCleared?.Invoke(this, waveId, lastWaveInLevel);
            else
                Debug.LogError("Boss wave cleared but no event is broadcasted :D");
        }
    }

    //////////////////////// public methods ////////////////////////
    public void StopGeneratingWaves()
    {
        StopAllCoroutines();
    }

    public void StartGeneratingWaves(LevelSettings levelSettings)
    {
        if (lastSpawnedEnemies.Count != 0)
            waveId = 0;

        powerupDropperSpawned = false;
        StopAllCoroutines();
        this.levelSettings = levelSettings;
        StartCoroutine(SpawnCoroutine());
    }

    public void ClearAllEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (!spawnedEnemies[i]) continue;

            spawnedEnemies[i].DestroyEnemy(false);
            spawnedEnemies.Remove(spawnedEnemies[i]);
        }

        var enemiesByTag = GameObject.FindGameObjectsWithTag(Tags.ENEMY_SHIP);
        for (int i = 0; i < enemiesByTag.Length; i++)
        {
            if (enemiesByTag[i] && enemiesByTag[i].TryGetComponent<EnemyAIBase>(out var enemyAIBase))
                enemyAIBase.DestroyEnemy();
            else
                Destroy(enemiesByTag[i]);
        }
    }

    public void SpawnBoss(GameObject bossPrefab)
    {
        StopGeneratingWaves();
        ClearAllEnemies();

        WaveEnemy boss = new()
        {
            enemyPrefab = bossPrefab,
            count = new(1, 1, 1),
            waveEnteringPositions = new WaveEnteringPosition[] { WaveEnteringPosition.ShouldEnterFromTop },
            enemyDifficulty = WaveEnemyDifficulty.Easy,
        };
        Spawn(boss);
    }
}