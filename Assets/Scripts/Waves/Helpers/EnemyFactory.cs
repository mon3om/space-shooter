using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Wave.Helper
{
    public class EnemyFactory : MonoBehaviour
    {
        public List<Transform> Spawn(WaveEnemy waveEnemy, int waveId, System.Action<EnemyAIBase, int> onEnemyDestroy)
        {
            List<Transform> spawned = new();
            List<EnemyAIBase> enemyAIBases = new();
            int enemiesCount = waveEnemy.count.RandomValue;

            float screenWidth = CameraUtils.CameraRect.xMax * 2;
            for (int i = 0; i < enemiesCount; i++)
            {
                var go = Instantiate(waveEnemy.enemyPrefab, new(CameraUtils.CameraRect.xMin + screenWidth / enemiesCount / 2f + screenWidth / enemiesCount * i, 10, 0), Quaternion.identity, transform);
                go.name += "-id=" + i + "-waveId=" + waveId;
                spawned.Add(go.transform);

                EnemyAIBase enemyAIBase = go.GetComponent<EnemyAIBase>();
                enemyAIBases.Add(enemyAIBase);
            }

            foreach (var item in enemyAIBases)
            {
                item.enemyIdentifier.waveId = waveId;
                item.enemyIdentifier.enemySpawnId = enemyAIBases.IndexOf(item);
                item.enemyIdentifier.waveEnemyDifficulty = waveEnemy.enemyDifficulty;
                item.waveSiblings = enemyAIBases;
                item.onEnemyDestroy += onEnemyDestroy;
            }

            return spawned;
        }

        public static List<Transform> SpawnNoWave(WaveEnemy waveEnemy, Transform parent)
        {
            List<Transform> spawned = new();
            List<EnemyAIBase> enemyAIBases = new();
            int enemiesCount = waveEnemy.count.RandomValue;

            float screenWidth = CameraUtils.CameraRect.xMax * 2;
            for (int i = 0; i < enemiesCount; i++)
            {
                var go = Instantiate(waveEnemy.enemyPrefab, new(CameraUtils.CameraRect.xMin + screenWidth / enemiesCount / 2f + screenWidth / enemiesCount * i, 10, 0), Quaternion.identity, parent);
                spawned.Add(go.transform);

                EnemyAIBase enemyAIBase = go.GetComponent<EnemyAIBase>();
                enemyAIBases.Add(enemyAIBase);
            }

            // foreach (var item in enemyAIBases)
            // {
            //     item.onEnemyDestroy += onEnemyDestroy;
            // }

            return spawned;
        }
    }
}