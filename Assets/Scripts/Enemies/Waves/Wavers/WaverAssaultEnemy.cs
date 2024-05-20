using UnityEngine;

public class WaverAssaultEnemy : WaverBase
{
    public StartEndPosition[] enemiesPositions;

    private void Start()
    {
        SpawnWave();
    }

    public override void SpawnWave()
    {
        for (int i = 0; i < enemiesPositions.Length; i++)
        {
            GameObject go = Instantiate(waveSettings.enemyPrefab, enemiesPositions[i].startPosition, Quaternion.identity, transform);
            ShootingAssaultEnemy shootingAssaultEnemy = go.GetComponent<ShootingAssaultEnemy>();
            shootingAssaultEnemy.destination = CameraUtils.GetPosition(enemiesPositions[i].endPosition);
            spawnedEnemies.Add(shootingAssaultEnemy);
        }
    }
}

[System.Serializable]
public class StartEndPosition
{
    public Vector2 startPosition;
    public CameraBasedPosition endPosition;
}
