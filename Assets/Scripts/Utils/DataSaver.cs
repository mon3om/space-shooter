using UnityEngine;

public class DataSaver
{
    public float GetDataFloat(string id)
    {
        if (!PlayerPrefs.HasKey(id))
        {
            Debug.LogWarning("Value not found");
            return -1;
        }

        return PlayerPrefs.GetFloat(id);
    }

    public float GetDataInt(string id)
    {
        if (!PlayerPrefs.HasKey(id))
        {
            Debug.LogWarning("Value not found");
            return -1;
        }

        return PlayerPrefs.GetInt(id);
    }

    public string GetDataString(string id)
    {
        if (!PlayerPrefs.HasKey(id))
        {
            Debug.LogWarning("Value not found");
            return "null";
        }

        return PlayerPrefs.GetString(id);
    }

    public void SaveDataString(string id, string data)
    {
        PlayerPrefs.SetString(id, data);
    }

    public void SaveDataFloat(string id, float data)
    {
        PlayerPrefs.SetFloat(id, data);
    }

    public void SaveDataInt(string id, int data)
    {
        PlayerPrefs.SetInt(id, data);
    }
}