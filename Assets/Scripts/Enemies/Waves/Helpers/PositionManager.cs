using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// CanHaveYPositionVariation,
// InVShape,
// InWaveShape,
// InIShape
// InRandomPositions

namespace Wave.Helper
{
    public class PositionManager
    {
        public void SetPositions(List<Transform> transforms, WaveEnemy waveEnemy)
        {
            if (waveEnemy.waveEnemyConditions.Contains(WaveEnemyCondition.InIShape))
            {
                // Get the positions of enemies based on their number and screen width
                float screenWidth = CameraUtils.CameraRect.xMax * 2;
                float enemyPositionWidth = screenWidth / transforms.Count;
                float startPosition = CameraUtils.CameraRect.xMin + enemyPositionWidth / 2f;

                for (int i = 0; i < transforms.Count; i++)
                {
                    Transform transform = transforms[i];
                    transform.position = new(startPosition + i * enemyPositionWidth, transform.position.y, 0);
                }
            }
            else
            {
                if (waveEnemy.waveEnemyConditions.Contains(WaveEnemyCondition.InWaveShape))
                {
                    // Get the positions of enemies based on their number and screen width
                    float screenWidth = CameraUtils.CameraRect.xMax * 2;
                    float enemyPositionWidth = screenWidth / transforms.Count;
                    float startPosition = CameraUtils.CameraRect.xMin + enemyPositionWidth / 2f;

                    for (int i = 0; i < transforms.Count; i++)
                    {
                        Transform transform = transforms[i];
                        transform.position = new(startPosition + i * enemyPositionWidth, transform.position.y - (i % 2 == 0 ? 2 : 0), 0);
                    }
                }
            }
        }

        public List<Vector2> GetAssaultEnemiesPositions(List<Transform> transforms, int maxEnemiesPerLine = 3)
        {
            int linesCount = Mathf.CeilToInt(transforms.Count / (float)maxEnemiesPerLine);

            float screenWidth = CameraUtils.CameraRect.xMax * 2;
            float screenHeight = CameraUtils.CameraRect.yMax * 1.5f;

            float enemyPositionWidth = screenWidth / maxEnemiesPerLine;
            float enemyPositionHeight = screenHeight / linesCount;
            Vector2 startPosition;
            List<Vector2> positions = new();
            int lineCounter = 0;
            int currentLineEnemiesCount;

            for (int j = 0; j < linesCount; j++)
            {
                currentLineEnemiesCount = transforms.Count - positions.Count >= maxEnemiesPerLine ? maxEnemiesPerLine : (transforms.Count - positions.Count) % maxEnemiesPerLine;
                enemyPositionWidth = screenWidth / currentLineEnemiesCount;
                startPosition = new(CameraUtils.CameraRect.xMin + enemyPositionWidth / 2f, CameraUtils.CameraRect.yMax - enemyPositionHeight / 2f);

                for (int i = 0; i < maxEnemiesPerLine; i++)
                {
                    Vector2 position = new(startPosition.x + i * enemyPositionWidth, startPosition.y - lineCounter * enemyPositionHeight);
                    positions.Add(position);
                    if (positions.Count == transforms.Count)
                        return positions;
                }

                lineCounter++;
            }

            return positions;
        }
    }
}