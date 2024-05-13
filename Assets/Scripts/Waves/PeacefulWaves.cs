using System.Collections.Generic;
using UnityEngine;

public class PeacefulWaves : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float ySpawnPosition = 10;
    [Space]
    public int enemiesCount = 8;
    public float distanceBetweenEnemies = 1;
    public float vShapeDistance = 0.5f;
    public WaveType waveType = WaveType.PeacefulLShape;

    private void Start()
    {
        SpawnWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            SpawnWave();
    }

    private List<Transform> spawnedEnemies = new List<Transform>();
    public void SpawnWave()
    {
        spawnedEnemies.Clear();

        for (int i = 0; i < enemiesCount; i++)
        {
            Transform t = Instantiate(enemyPrefab, Vector3.up * ySpawnPosition, Quaternion.identity).transform;
            spawnedEnemies.Add(t);
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
            spawnedEnemies[i].position = new Vector3(xStart + distanceBetweenEnemies * i, spawnedEnemies[i].position.y, 0);
    }

    private void ArrangeVShape(bool inversed)
    {
        float xStart = -((enemiesCount - 1) * distanceBetweenEnemies) / 2f;
        float lastYPosition = spawnedEnemies[0].position.y;

        for (int i = 0; i < enemiesCount; i++)
        {
            float yPos = lastYPosition - vShapeDistance * (i > enemiesCount / 2 ? -1 : 1) * (inversed ? -1 : 1);
            lastYPosition = yPos;
            spawnedEnemies[i].position = new Vector3(xStart + distanceBetweenEnemies * i, yPos, 0);
        }
    }

    private void ArrangeWaveShape()
    {
        float xStart = -((enemiesCount - 1) * distanceBetweenEnemies) / 2f;

        for (int i = 0; i < enemiesCount; i++)
        {
            float yPos = spawnedEnemies[i].position.y + (i % 2 == 0 ? vShapeDistance : 0);
            spawnedEnemies[i].position = new Vector3(xStart + distanceBetweenEnemies * i, yPos, 0);
        }
    }
}