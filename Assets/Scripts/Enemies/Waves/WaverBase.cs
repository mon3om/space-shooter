using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    PeacefulVShape, PeacefulLShape, PeacefulWaveShape, PeacefulInversedVShape
}

public abstract class WaverBase : MonoBehaviour
{
    [HideInInspector] public WaveSettings waveSettings;
    public float delayAfterLastWave = 10f;
    protected List<EnemyAIBase> spawnedEnemies = new();
    public System.Action onAllEnemiesDestroyed;

    protected void RemoveEnemy(EnemyAIBase enemy)
    {
        spawnedEnemies.Remove(enemy);
        if (spawnedEnemies.Count == 0)
            onAllEnemiesDestroyed?.Invoke();
    }

    public abstract void SpawnWave();
}