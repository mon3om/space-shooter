using System;
using UnityEngine;
using UnityEngine.Events;

public enum PowerupCategory { None, Weapon, Stackable }

[CreateAssetMenu(fileName = "Powerup", menuName = "Powerup")]
public class PowerupScriptableObject : ScriptableObject
{
    public int id { get { return GetId(); } }
    public Sprite sprite;
    public string itemName { get { return GetName(); } }
    public string description;
    public bool available = false;
    public PowerupCategory powerupCategory;
    public PowerupScriptableObject[] requiredPowerups; // At least one of these powerups should be equipped in order to instantiate this powerup

    [Space]
    [Header("Optional animation")]
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
        throw new Exception("Script couldn't be fetched");
    }

    private string GetName()
    {
        return name.Substring(name.IndexOf("-") + 1).Trim();
    }

    private int GetId()
    {
        int _id;
        try
        {
            _id = int.Parse(name.Substring(0, name.IndexOf("-")).Trim());
            return _id;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            Debug.LogError("On item with name = " + name);
            return -1;
        }
    }

    public bool HasRequiredPowerups()
    {
        bool canEquipe = true;
        if (requiredPowerups.Length > 0)
        {
            canEquipe = false;
            foreach (var equipped in PowerupsManager.equippedPowerups)
            {
                foreach (var required in requiredPowerups)
                {
                    if (equipped.id == required.id)
                    {
                        canEquipe = true;
                        break;
                    }
                }
                if (canEquipe)
                    break;
            }
        }

        return canEquipe;
    }

    public Action OnPowerupActivated;
}
