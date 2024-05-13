using UnityEngine;

public enum Actor { Player = 0, Enemy }

public class Utils
{
    public static Quaternion GetLookAtRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}

[System.Serializable]
public class RandomnessHelper
{
    public float minValue;
    public float maxValue;

    public float GetRandomValue()
    {
        float value = Random.Range(minValue, maxValue);
        return value;
    }
}