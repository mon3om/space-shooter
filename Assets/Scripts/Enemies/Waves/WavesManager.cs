using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Wave.Helper;

[RequireComponent(typeof(EnemyFactory))]
public class WavesManager : MonoBehaviour
{
    public float timeBetweenWaves = 5f;
    public List<WaveEnemy> waveEnemies = new();
    [HideInInspector] public int waveId = 0;
    //
    private EnemyFactory enemyFactory;
    private PositionManager positionManager = new();

    private List<EnemyAIBase> spawnedEnemies = new();

    public static WavesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        enemyFactory = GetComponent<EnemyFactory>();

        StartCoroutine(SpawnNewWave());
    }

    private IEnumerator SpawnNewWave()
    {
        var pickedEnemy = PickRandomItem(waveEnemies);
        var spawned = enemyFactory.Spawn(pickedEnemy, waveId);
        foreach (var item in spawned)
        {
            spawnedEnemies.Add(item.GetComponent<EnemyAIBase>());
        }

        positionManager.SetPositions(spawned, pickedEnemy);
        yield return new WaitForSeconds(timeBetweenWaves);
        waveId++;
        StartCoroutine(SpawnNewWave());
    }

    private WaveEnemy PickRandomItem(List<WaveEnemy> items)
    {
        // Calculate the total weight
        float totalWeight = 0f;
        foreach (WaveEnemy item in items)
        {
            totalWeight += item.Weight;
        }

        // Generate a random number between 0 and the total weight
        float randomValue = Random.Range(0, totalWeight);

        // Iterate through the items and select based on the random value
        float cumulativeWeight = 0f;
        foreach (WaveEnemy item in items)
        {
            cumulativeWeight += item.Weight;
            if (randomValue < cumulativeWeight)
            {
                return item;
            }
        }

        // In case of rounding errors, return the last item
        return items[0];
    }

    public List<EnemyAIBase> GetEnemiesByTypeAndWaveId<T>(int waveId)
    {
        List<EnemyAIBase> enemies = spawnedEnemies.Where(el => el.GetType() == typeof(T) && el.waveId == waveId).ToList();
        return enemies;
    }
}

class WaveData
{
    public WaveEnemy waveEnemy;
    public float yStartPosition;
    public float yEndPosition;
}

public enum WaveEnemyDifficulty { Easy, Moderate, Hard }
public enum WaveEnemyCondition
{
    InSingle, InPackOf2, InPackOf3,
    InPackOf4,
    InPackOf5,
    InPackOf6,
    InPackOf7,
    InPackOf8,
    InPackOf9,
    InPackOf10,
    InPackOf11,
    InPackOf12,
    CanHaveYPositionVariation,
    InVShape,
    InWaveShape,
    InIShape,
    InRandomPositions
}
[System.Serializable]
public class WaveEnemy
{
    public GameObject enemyPrefab;
    public WaveEnemyDifficulty enemyDifficulty = WaveEnemyDifficulty.Easy;
    [HideInInspector]
    public float Weight
    {
        set
        {
            Weight = value;
        }
        // TODO update this code depending on game difficulty
        get
        {
            return enemyDifficulty switch
            {
                WaveEnemyDifficulty.Easy => 40,
                WaveEnemyDifficulty.Moderate => 20,
                WaveEnemyDifficulty.Hard => 8,
                _ => (float)0,
            };
        }
    }

    public WaveEnemyCondition[] waveEnemyConditions;

    [Space]
    public WaveEnemy[] compatibleEnemies;
}