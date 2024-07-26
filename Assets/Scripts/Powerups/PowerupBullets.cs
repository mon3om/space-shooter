using UnityEngine;

public class PowerupBullets : PowerupBase
{
    protected override void CustomActivate()
    {
        var bullets = Resources.Load<ShootingSettings>("ScriptableObjects/Shooting/Player");
        playerInstance.GetComponent<ShootingBase>().shootingSettings = bullets;
    }

    protected override void CustomDeactivate()
    {
        var missiles = Resources.Load<ShootingSettings>("ScriptableObjects/Shooting/Player-HomingMissile");
        playerInstance.GetComponent<ShootingBase>().shootingSettings = missiles;
    }
}
