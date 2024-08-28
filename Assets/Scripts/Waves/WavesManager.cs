using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsonWaves;
using UnityEngine;
using Wave.Helper;


public class WavesManager : MonoBehaviour
{
    [HideInInspector] public LevelSettings levelSettings;
    [HideInInspector] public int waveId = 0;
    //

    public static WavesManager Instance;

    private EnemyFactory enemyFactory;
    [HideInInspector] public List<EnemyAIBase> spawnedEnemies = new();
    private List<string> lastSpawnedEnemies = new();
    private List<string> lastSpawnedDifficulty = new();

    private List<WaveEnemy> easyEnemies = new();
    private List<WaveEnemy> hardEnemies = new();
    private List<WaveEnemy> moderateEnemies = new();

    [HideInInspector] public int levelIndex = 0;

    // Events
    public System.Action<WavesManager, int> onWaveCleared; // <this, waveId>
    public System.Action<int> onWaveSpawned; // <waveId>
    public System.Action onMiniBossDefeated;

    private List<WaveEnemy> waveEnemies;
    private bool powerupDropperSpawned = false;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
#if UNITY_EDITOR
        await JsonWavesManager.StartNode();
        await System.Threading.Tasks.Task.Delay(500);
#endif

        waveEnemies = JsonWavesManager.GetWaves();

        easyEnemies = waveEnemies.Where(el => el.enemyDifficulty == WaveEnemyDifficulty.Easy).ToList();
        moderateEnemies = waveEnemies.Where(el => el.enemyDifficulty == WaveEnemyDifficulty.Moderate).ToList();
        hardEnemies = waveEnemies.Where(el => el.enemyDifficulty == WaveEnemyDifficulty.Hard).ToList();

        enemyFactory = GetComponentInChildren<EnemyFactory>();
    }

    private IEnumerator SpawnCoroutine()
    {
        var pickedEnemy = GetSingleEnemyDifficultyBased();
        Spawn(pickedEnemy);
        // SpawnPowerupDropper(); // Conditions are checked withing the function

        if (waveId == Mathf.RoundToInt(levelSettings.wavesCount))
        {
            Debug.Log("Stopped spawning because maximum waves count is reached");
            yield break;
        }

        yield return new WaitForSeconds(levelSettings.timeBetweenWaves);
        StartCoroutine(SpawnCoroutine());
    }

    [Space]
    public GameObject temp_powerupDropper;
    private void SpawnPowerupDropper()
    {
        if (powerupDropperSpawned) return;

        if ((levelIndex - 2) % 3 == 0 && waveId > levelSettings.wavesCount / 2)
        {
            WaveEnemy waveEnemy = new WaveEnemy();
            waveEnemy.enemyPrefab = temp_powerupDropper;
            waveEnemy.waveEnteringPositions = new WaveEnteringPosition[] { WaveEnteringPosition.ShouldEnterFromTop };
            waveEnemy.count = new IntRange(1, 1, 1);
            Spawn(waveEnemy);
            powerupDropperSpawned = true;

        }
    }

    private void Spawn(WaveEnemy pickedEnemy)
    {
        var spawned = enemyFactory.Spawn(pickedEnemy, waveId, OnEnemyDestroy);
        foreach (var item in spawned)
            spawnedEnemies.Add(item.GetComponent<EnemyAIBase>());

        PositionManager.SetPositions(spawned, pickedEnemy);
        onWaveSpawned?.Invoke(waveId);
        waveId++;
    }

    private List<WaveEnemy> GetEnemiesDifficultyBased()
    {
        List<WaveEnemy> enemies;
        string diff = "Easy";

        float random = Random.Range(0, 101);
        if (random < levelSettings.difficultySettings.hardWeight)
        {
            enemies = hardEnemies;
            diff = "Hard";
        }
        else if (random < levelSettings.difficultySettings.moderateWeight)
        {
            enemies = moderateEnemies;
            diff = "Moderate";
        }
        else
        {
            enemies = easyEnemies;
        }

        // Debug.Log("PICKED = " + diff);

        // if (lastSpawnedDifficulty.All(el => el == diff) && lastSpawnedDifficulty.Count > 1)
        //     return GetEnemiesDifficultyBased();
        // else lastSpawnedDifficulty.Add(diff);

        // if (lastSpawnedDifficulty.Count > 2) lastSpawnedDifficulty.RemoveAt(0);

        return enemies;
    }

    private WaveEnemy GetSingleEnemyDifficultyBased()
    {
        var pickedEnemies = GetEnemiesDifficultyBased();
        var picked = pickedEnemies[Random.Range(0, pickedEnemies.Count)];

        if ((lastSpawnedEnemies.Contains(picked.enemyPrefab.name) && lastSpawnedEnemies.Count > 1) ||
        (lastSpawnedEnemies.Count > 0 && lastSpawnedEnemies[^1] == picked.enemyPrefab.name))
            return GetSingleEnemyDifficultyBased();
        else
            lastSpawnedEnemies.Add(picked.enemyPrefab.name);

        if (lastSpawnedEnemies.Count > 2) lastSpawnedEnemies.RemoveAt(0);

        return picked;
    }

    public List<EnemyAIBase> GetEnemiesByTypeAndWaveId<T>(int waveId)
    {
        List<EnemyAIBase> enemies = spawnedEnemies.Where(el => el.GetType() == typeof(T) && el.enemyIdentifier.waveId == waveId).ToList();
        return enemies;
    }

    private void OnEnemyDestroy(EnemyAIBase enemyAIBase, int waveId)
    {
        spawnedEnemies.Remove(enemyAIBase);

        // If wave is cleared
        if (spawnedEnemies.Where(_ => _.enemyIdentifier.waveId == waveId).Count() == 0)
        {
            onWaveCleared?.Invoke(this, waveId);
        }
    }

    //////////////////////// public methods ////////////////////////
    public void StopGeneratingWaves()
    {
        StopAllCoroutines();
    }

    public void StartGeneratingWaves(LevelSettings levelSettings)
    {
        powerupDropperSpawned = false;
        StopAllCoroutines();
        this.levelSettings = levelSettings;
        waveId = 0;
        StartCoroutine(SpawnCoroutine());
    }

    public void ClearAllEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            spawnedEnemies.Remove(spawnedEnemies[i]);
            spawnedEnemies[i].DestroyEnemy(false);
        }
    }

    public void SpawnBoss(GameObject bossPrefab)
    {
        StopGeneratingWaves();
        ClearAllEnemies();

        WaveEnemy waveEnemy = new()
        {
            enemyPrefab = bossPrefab,
            count = new(1, 1, 1),
            waveEnteringPositions = new WaveEnteringPosition[] { WaveEnteringPosition.ShouldEnterFromTop },
            enemyDifficulty = WaveEnemyDifficulty.Easy,
        };
        Spawn(waveEnemy);
    }
}