using System;
using UnityEngine;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "Powerup", menuName = "Powerup")]
public class PowerupScriptableObject : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public string itemName;
    public string description;
    public bool available = false;

    [Space]
    [Header("Optional aimation")]
    public AnimationClip animationClip;

    [Space]
    public TextAsset scriptTypeName;

    public Type GetPowerupType()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.LastPartOfTypeName() == scriptTypeName.name)
                {
                    return type;
                }
            }
        }
        return null;
    }

    private void OnValidate()
    {
        if (available && scriptTypeName == null)
            Debug.LogError(name + " doesn't have a reference for the scriptTypeName!");
    }
}
