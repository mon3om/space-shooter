
using UnityEngine;

[System.Serializable]
public class EnemyIdentifier
{
    public WaveEnemyDifficulty waveEnemyDifficulty;
    public WaveShape waveShape;
    public WaveEnteringPosition waveEnteringPosition;
    public int enemySpawnId;
    public int waveId;
    public Vector2 initPosition;
    public Vector2 targetPosition;
}

public enum WaveEnemyDifficulty { Easy, Moderate, Hard }
public enum WaveShape
{
    InRandomYPosition,
    InVShape,
    InWaveShape,
    InIShape,
}
public enum WaveEnteringPosition
{
    ShouldEnterFromTop,
    ShouldEnterFromBottom,
    ShouldEnterFromLeft,
    ShouldEnterFromRight,
    ShouldEnterFromVerticalCornersMiddle,
    ShouldEnterFromVerticalCornersLeft,
    ShouldEnterFromVerticalCornersRight,
    ShouldEnterFromHorizontalCornersTop,
    ShouldEnterFromHorizontalCornersCenter,
    ShouldEnterFromHorizontalCornersBottom,
    ShouldEnterFromRandom
}
[System.Serializable]
public class WaveEnemy
{
    public GameObject enemyPrefab;
    public WaveEnemyDifficulty enemyDifficulty = WaveEnemyDifficulty.Easy;
    public IntRange count;

    [Space]
    public WaveEnteringPosition[] waveEnteringPositions;
    public WaveShape[] waveShapes;

    [Space]
    public int maxEnemiesPerLine = -1;
    public WaveEnemy[] compatibleEnemies;
    public int interval = -1;
    public int intervalRepeat = -1;
}

[System.Serializable]
public class IntRange
{
    [SerializeField] private int defaultValue;
    [SerializeField] private int maxIfRange = -1;
    [SerializeField] private int step = -1;


    public IntRange(int min, int max, int step)
    {
        defaultValue = min;
        maxIfRange = max;
        this.step = step;
    }

    public int RandomValue
    {
        get
        {
            int calculatedRandomRange;
            if (maxIfRange == -1) calculatedRandomRange = defaultValue;
            else if (step == -1) calculatedRandomRange = Random.Range(defaultValue, maxIfRange + 1);
            else
            {
                int randomStepIndex = Random.Range(0, Mathf.CeilToInt((maxIfRange - defaultValue) / (float)step));
                calculatedRandomRange = defaultValue + step * randomStepIndex;
            }

            return calculatedRandomRange;
        }
    }


}