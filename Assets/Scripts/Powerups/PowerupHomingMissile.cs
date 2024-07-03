using UnityEngine;

public class PowerupHomingMissile : PowerupBase
{
    public ShootingSettings bullets, missiles;

    protected override void CustomActivate()
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings = missiles;
    }

    protected override void CustomDeactivate()
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings = bullets;
    }
}