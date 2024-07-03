
public class PowerupPiercing : PowerupBase
{
    protected override void CustomActivate()
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings.isPiercing = true;
    }

    protected override void CustomDeactivate()
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings.isPiercing = false;
    }
}