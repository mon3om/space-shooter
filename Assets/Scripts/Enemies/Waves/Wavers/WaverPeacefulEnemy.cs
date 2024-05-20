using System.Collections.Generic;
using UnityEngine;

public class WaverPeacefulEnemy : WaverBase
{
    [Space]
    public Vector3 ySpawnPosition = new(0, 6, 0);
    public int enemiesCount = 8;
    public float distanceBetweenEnemies = 1;
    public float vShapeDistance = 0.5f;
    public WaveType waveType = WaveType.PeacefulLShape;
    [Space]
    public float vShapeYPositionOsset = 4f;

    private void Start()
    {
        SpawnWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            SpawnWave();
    }

    public override void SpawnWave()
    {
        spawnedEnemies.Clear();

        if (waveType == WaveType.PeacefulVShape) ySpawnPosition.y += vShapeYPositionOsset;
        for (int i = 0; i < enemiesCount; i++)
        {
            Transform t = Instantiate(waveSettings.enemyPrefab, Vector3.up * ySpawnPosition.y, Quaternion.identity, transform).transform;
            spawnedEnemies.Add(t.GetComponent<EnemyAIBase>());
            t.name = "Peaceful-" + i;
        }

        switch (waveType)
        {
            case WaveType.PeacefulLShape:
                ArrangeLShape();
                break;
            case WaveType.PeacefulVShape:
                ArrangeVShape(false);
                break;
            case WaveType.PeacefulInversedVShape:
                ArrangeVShape(true);
                break;
            case WaveType.PeacefulWaveShape:
                ArrangeWaveShape();
                break;
            default:
                break;
        }
    }

    private void ArrangeLShape()
    {
        float xStart = -((enemiesCount - 1) * distanceBetweenEnemies) / 2f;
        for (int i = 0; i < enemiesCount; i++)
            spawnedEnemies[i].transform.position = new Vector3(xStart + distanceBetweenEnemies * i, spawnedEnemies[i].transform.position.y, 0);
    }

    private void ArrangeVShape(bool inversed)
    {
        float xStart = -((enemiesCount - 1) * distanceBetweenEnemies) / 2f;
        float lastYPosition = spawnedEnemies[0].transform.position.y;

        for (int i = 0; i < enemiesCount; i++)
        {
            float yPos = lastYPosition - vShapeDistance * (i > enemiesCount / 2 ? -1 : 1) * (inversed ? -1 : 1);
            lastYPosition = yPos;
            spawnedEnemies[i].transform.position = new Vector3(xStart + distanceBetweenEnemies * i, yPos, 0);
        }
    }

    private void ArrangeWaveShape()
    {
        float xStart = -((enemiesCount - 1) * distanceBetweenEnemies) / 2f;

        for (int i = 0; i < enemiesCount; i++)
        {
            float yPos = spawnedEnemies[i].transform.position.y + (i % 2 == 0 ? vShapeDistance : 0);
            spawnedEnemies[i].transform.position = new Vector3(xStart + distanceBetweenEnemies * i, yPos, 0);
        }
    }
}