using UnityEngine;

public class PowerupPlusOne : PowerupBase
{
    protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings.shotsCount++;
    }

    protected override void CustomDeactivate()
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings.shotsCount--;
        if (playerInstance.GetComponent<ShootingBase>().shootingSettings.shotsCount < 1)
            playerInstance.GetComponent<ShootingBase>().shootingSettings.shotsCount = 1;
    }
}