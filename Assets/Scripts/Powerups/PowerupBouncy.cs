
public class PowerupBouncy : PowerupBase
{
    protected override void CustomActivate()
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings.isBouncy = true;
    }

    protected override void CustomDeactivate()
    {
        playerInstance.GetComponent<ShootingBase>().shootingSettings.isBouncy = false;
    }
}