using System.Linq;
using UnityEngine;

public abstract class PowerupBase : MonoBehaviour
{
    public static System.Action<PowerupBase> OnPowerupReady;
    [HideInInspector] public Transform playerInstance;
    [HideInInspector] public PowerupScriptableObject powerupScriptableObject;
    public System.Action ReplacePowerup;

    protected void Start()
    {
        OnPowerupReady?.Invoke(this);
        name = powerupScriptableObject.itemName;
    }

    public PowerupBase Activate()
    {
        // Remove all equipped weapons before equipping a new weapon
        if (powerupScriptableObject.powerupCategory == PowerupCategory.Weapon)
            PowerupsManager.equippedPowerups.RemoveAll(el => el.powerupCategory == PowerupCategory.Weapon);

        if (PowerupsManager.equippedPowerups.Any(el => el.id == powerupScriptableObject.id) == false)
            PowerupsManager.equippedPowerups.Add(powerupScriptableObject);

        if (!playerInstance) OnPowerupReady?.Invoke(this);
        BaseActivate();
        powerupScriptableObject.OnPowerupActivated?.Invoke();
        powerupScriptableObject.OnPowerupActivated = null;

        Instances.UIEquipedPanel.DisplayEquiped();
        Instances.LevelManager?.OnBossDefeated();

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
