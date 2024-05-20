using UnityEngine;

public class WaverPeacefulTracker : WaverBase
{
    [Space]
    public int enemiesCount = 8;
    public Vector3 spawnPosition;
    public Vector3 stopPoint;
    public float distanceBetweenEnemies = 1f;
    public float timeBetweenTrackingWaves = 3f;

    void Start()
    {
        SpawnWave();
    }

    private void ArrangeLShape()
    {
        float xStart = -((enemiesCount - 1) * distanceBetweenEnemies) / 2f;
        for (int i = 0; i < enemiesCount; i++)
            spawnedEnemies[i].transform.position = new Vector3(xStart + distanceBetweenEnemies * i, spawnedEnemies[i].transform.position.y, 0);
    }

    public override void SpawnWave()
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            GameObject go = Instantiate(waveSettings.enemyPrefab, spawnPosition, Quaternion.identity, transform);
            PeacefulTrackerEnemy peacefulTrackerEnemy = go.GetComponent<PeacefulTrackerEnemy>();
            peacefulTrackerEnemy.stoppingPoint = stopPoint;
            spawnedEnemies.Add(peacefulTrackerEnemy);
            go.name = "PeacefulTracker-" + i;
        }

        ArrangeLShape();

        for (int i = 0; i < enemiesCount; i++)
        {
            spawnedEnemies[i].GetComponent<PeacefulTrackerEnemy>().waitTimeBeforeTrackingPlayer = (i + 1) * timeBetweenTrackingWaves;
            if (enemiesCount % 2 == 0)
            {
                spawnedEnemies[enemiesCount - 1 - i].GetComponent<PeacefulTrackerEnemy>().waitTimeBeforeTrackingPlayer = (i + 1) * timeBetweenTrackingWaves;
                if (i == enemiesCount / 2 - 1)
                    return;
            }
            else
            {
                Debug.Log("Odd enemies count");
                if (i + 1 > enemiesCount / 2f)
                    return;

                spawnedEnemies[enemiesCount - 1 - i].GetComponent<PeacefulTrackerEnemy>().waitTimeBeforeTrackingPlayer = (i + 1) * timeBetweenTrackingWaves;
            }
        }
    }
}
