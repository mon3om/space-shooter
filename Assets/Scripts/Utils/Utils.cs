using UnityEngine;

public enum Actor { Player = 0, Enemy }

public class Utils
{
    public static Quaternion GetLookAtRotation(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    public static string GetAllTagsAsVariableDeclaration()
    {
        string str = "";
        foreach (var item in UnityEditorInternal.InternalEditorUtility.tags)
        {
            str += "private static readonly string " + item + " = " + "\"" + item + "\";\n";
        }
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