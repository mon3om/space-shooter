using UnityEngine;

[System.Serializable]
public class RandomEvent
{
    public float randomChance;

    public bool EventWillHappen()
    {
        return Random.Range(0f, 100f) <= randomChance;
    }
}