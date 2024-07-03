using UnityEngine;

public class PowerupPlusOne : PowerupBase
{
    protected override void CustomActivate()
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