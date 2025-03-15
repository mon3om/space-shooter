using System.Collections.Generic;
using System.Linq;
using JsonWaves;
using UnityEngine;

public class WaveEnemyPicker : MonoBehaviour
{
    private List<WaveEnemy> waveEnemies;
    private List<WaveEnemy> easyEnemies = new();
    private List<WaveEnemy> hardEnemies = new();
    private List<WaveEnemy> moderateEnemies = new();

    public async void Start()
    {
#if UNITY_EDITOR
        await JsonWavesManager.StartNode();
        await System.Threading.Tasks.Task.Delay(500);
#endif

        waveEnemies = JsonWavesManager.GetWaves();
        easyEnemies = waveEnemies.Where(el => el.enemyDifficulty == WaveEnemyDifficulty.Easy).ToList();
        moderateEnemies = waveEnemies.Where(el => el.enemyDifficulty == WaveEnemyDifficulty.Moderate).ToList();
        hardEnemies = waveEnemies.Where(el => el.enemyDifficulty == WaveEnemyDifficulty.Hard).ToList();
    }

    public WaveEnemy GetSingleEnemyDifficultyBased(LevelSettings levelSettings, List<string> lastSpawnedEnemies)
    {
        var pickedEnemies = GetEnemiesDifficultyBased(levelSettings);
        var picked = pickedEnemies[Random.Range(0, pickedEnemies.Count)];

        if ((lastSpawnedEnemies.Contains(picked.enemyPrefab.name) && lastSpawnedEnemies.Count > 1) ||
        (lastSpawnedEnemies.Count > 0 && lastSpawnedEnemies[^1] == picked.enemyPrefab.name))
            return GetSingleEnemyDifficultyBased(levelSettings, lastSpawnedEnemies);
        else
        {
            lastSpawnedEnemies.Add(picked.enemyPrefab.name.Replace("1", "").Replace("2", ""));
        }

        if (lastSpawnedEnemies.Count > 2) lastSpawnedEnemies.RemoveAt(0);

        return picked;
    }

    private List<WaveEnemy> GetEnemiesDifficultyBased(LevelSettings levelSettings)
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

    public List<EnemyAIBase> GetEnemiesByTypeAndWaveId<T>(int waveId, List<EnemyAIBase> spawnedEnemies)
    {
        List<EnemyAIBase> enemies = spawnedEnemies.Where(el => el.GetType() == typeof(T) && el.enemyIdentifier.waveId == waveId).ToList();
        return enemies;
    }
}