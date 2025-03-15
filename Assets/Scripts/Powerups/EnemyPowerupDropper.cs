using System.Collections.Generic;
using UnityEngine;

public class EnemyPowerupDropper : MonoBehaviour
{
    public GameObject powerupDropperPrefab;
    public int powerupsCount = 1;
    [Space]
    public List<int> powerupsIdForDebug;

    private EnemyAIBase enemyAIBase;

    private void Start()
    {
        if (TryGetComponent(out enemyAIBase))
        {
            enemyAIBase.onEnemyDestroy += (enemy, waveId) =>
            {
                if (enemy.killedByPlayer)
                    GeneratePowerups();
            };
        }
        else
        {
            Debug.LogWarning("Can't get component EnemyAIBase, Please attach it to the enemy named " + name);
        }
    }

    public void GeneratePowerups()
    {
        PowerupDropperPrefab pudpComponent = Instantiate(powerupDropperPrefab, transform.position, Quaternion.identity).GetComponent<PowerupDropperPrefab>();
        pudpComponent.powerupsCount = powerupsCount;
        pudpComponent.powerupsIdForDebug = powerupsIdForDebug;
    }
}
