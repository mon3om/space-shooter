using System.Linq;
using UnityEngine;

public abstract class PowerupBase : MonoBehaviour
{
    public static System.Action<PowerupBase> OnPowerupReady;
    [HideInInspector] public Transform playerInstance;
    [HideInInspector] public PowerupScriptableObject powerupScriptableObject;

    private void Start()
    {
        OnPowerupReady?.Invoke(this);
    }

    public PowerupBase Activate(PowerupScriptableObject powerupScriptableObject)
    {
        Debug.Log("ACTIVATING");
        // Remove all equipped weapons before equipping a new weapon
        if (powerupScriptableObject.powerupCategory == PowerupCategory.Weapon)
            PowerupsManager.equippedPowerups.RemoveAll(el => el.powerupCategory == PowerupCategory.Weapon);

        Debug.Log("00");
        if (PowerupsManager.equippedPowerups.Any(el => el.id == powerupScriptableObject.id) == false)
            PowerupsManager.equippedPowerups.Add(powerupScriptableObject);

        Debug.Log("11");
        PowerupsManager.equippedPowerups.ForEach(el => Debug.Log(el.itemName));

        if (!playerInstance) OnPowerupReady?.Invoke(this);
        Debug.Log("22");
        BaseActivate();
        Debug.Log("BASE ACTIVATE DONE");
        powerupScriptableObject.OnPowerupActivated?.Invoke();
        powerupScriptableObject.OnPowerupActivated = null;
        return this;
    }
    public PowerupBase Deactivate()
    {
        if (!playerInstance) OnPowerupReady?.Invoke(this);
        BaseDeactivate();

        // Remove the powerup from equipped powerups
        PowerupsManager.equippedPowerups.RemoveAll(el => el.id == powerupScriptableObject.id);
        return this;
    }

    protected abstract void CustomActivate(PowerupScriptableObject powerupScriptableObject);
    protected abstract void CustomDeactivate();

    private void BaseActivate()
    {
        CustomActivate(powerupScriptableObject);
    }
    private void BaseDeactivate()
    {
        CustomDeactivate();
    }
}
