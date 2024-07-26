using UnityEngine;

public class PowerupHomingMissile : PowerupBase
{
    protected override void CustomActivate()
    {
        var missiles = Resources.Load<ShootingSettings>("ScriptableObjects/Shooting/Player-HomingMissile");
        playerInstance.GetComponent<ShootingBase>().shootingSettings = missiles;
    }

    protected override void CustomDeactivate()
    {
        var bullets = Resources.Load<ShootingSettings>("ScriptableObjects/Shooting/Player");
        playerInstance.GetComponent<ShootingBase>().shootingSettings = bullets;
    }
}