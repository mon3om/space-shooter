using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Wave.Helper
{
    public class EnemyFactory : MonoBehaviour
    {
        public List<Transform> Spawn(WaveEnemy waveEnemy, int waveId)
        {
            List<Transform> spawned = new();
            List<EnemyAIBase> enemyAIBases = new();
            int enemiesCount = 0;

            if (waveEnemy.waveEnemyConditions.Contains(WaveEnemyCondition.InPackOf5))
            {
                enemiesCount = 5;
            }
            else if (waveEnemy.waveEnemyConditions.Contains(WaveEnemyCondition.InPackOf6))
            {
                enemiesCount = 6;
            }

            for (int i = 0; i < enemiesCount; i++)
            {
                var go = Instantiate(waveEnemy.enemyPrefab, new(0, 10, 0), Quaternion.identity, transform);
                go.name = "" + i;
                spawned.Add(go.transform);

                EnemyAIBase enemyAIBase = go.GetComponent<EnemyAIBase>();
                enemyAIBases.Add(enemyAIBase);
            }

            foreach (var item in enemyAIBases)
            {
                item.waveId = waveId;
                item.waveSiblings = enemyAIBases;
            }

            return spawned;
        }

    }
}