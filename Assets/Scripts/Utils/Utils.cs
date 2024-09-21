using System.Collections.Generic;
using UnityEngine;

public enum Actor { Player = 0, Enemy }

public class Utils
{
    public const float MIN_Y_POSITION = 3f;
    public static Quaternion GetLookAtRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    public static Quaternion GetLookAtRotation(Vector3 objectPosition, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - objectPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    public static string GetAllTagsAsVariableDeclaration()
    {
        string str = "";

#if UNITY_EDITOR
        foreach (var item in UnityEditorInternal.InternalEditorUtility.tags)
        {
            str += "private static readonly string " + item + " = " + "\"" + item + "\";\n";
        }
#endif
        Debug.Log(str);
        return str;
    }

    public bool CompareMultipleTags(Collider2D other, params string[] tags)
    {
        foreach (var item in tags)
        {
            if (other.CompareTag(item))
                return true;
        }

        return false;
    }

    public static List<Vector2> DivideScreen(float xSteps, float ySteps, float minYPos = MIN_Y_POSITION)
    {
        var screenPositions = new List<Vector2>();

        float areaWidth = CameraUtils.CameraRect.xMax * 2 / xSteps;
        float areaHeight = (CameraUtils.CameraRect.yMax * 2 - minYPos) / ySteps;

        for (int i = 0; i < xSteps; i++)
            for (int j = 0; j < ySteps; j++)
                screenPositions.Add(new(i * areaWidth - areaWidth * xSteps / 2f + areaWidth / 2, CameraUtils.CameraRect.yMax - j * areaHeight - areaHeight / 2));

        return screenPositions;
    }
}

[System.Serializable]
public class RandomFloat
{
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;
    public float value { get { return GetRandomValue(); } }

    public RandomFloat(float minValue, float maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    private float GetRandomValue()
    {
        float value = Random.Range(minValue, maxValue);
        return value;
    }
}


[System.Serializable]
public class LevelSettings
{
    public float timeBetweenWaves = 5;
    public float floatWavesCount = 5;
    public int WavesCount { get { return Mathf.RoundToInt(floatWavesCount); } }
    public LevelDifficultySettings difficultySettings;
    public BossSettings bossSettings;

    public override string ToString()
    {
        var temp = "";
        temp += "timeBetweenWaves = " + timeBetweenWaves + "\n";
        temp += "floatWavesCount = " + floatWavesCount + "\n";
        temp += "easy = " + (difficultySettings.moderateWeight + difficultySettings.hardWeight) + "%\n";
        temp += "moderate = " + difficultySettings.moderateWeight + "%\n";
        temp += "hard = " + difficultySettings.hardWeight + "%\n";

        return temp;
    }

    public LevelSettings IncreaseDifficulty(LevelSettings newLevel, LevelSettings previousLevel)
    {
        var diffIncrease = newLevel.difficultySettings.difficultyIncreasePercent;

        newLevel.timeBetweenWaves = previousLevel.timeBetweenWaves * ((100 - diffIncrease) / 100);
        newLevel.difficultySettings.moderateWeight = previousLevel.difficultySettings.moderateWeight * (100 + diffIncrease) / 100;
        newLevel.difficultySettings.hardWeight = previousLevel.difficultySettings.hardWeight * (100 + diffIncrease) / 100;
        newLevel.floatWavesCount = previousLevel.floatWavesCount * ((100 + diffIncrease) / 100);

        return newLevel;
    }
}

[System.Serializable]
public class BossSettings
{
    public GameObject bossPrefab;
    public AudioClip alertClip;
    public AudioClip bossClip;
}

[System.Serializable]
public class LevelDifficultySettings
{
    public float moderateWeight;
    public float hardWeight;
    public float difficultyIncreasePercent = 3;
}