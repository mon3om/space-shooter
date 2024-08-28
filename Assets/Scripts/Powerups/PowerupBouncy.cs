
public class PowerupBouncy : PowerupBase
{
    protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
    {
        PowerupsManager.bounceCount++;
    }

    protected override void CustomDeactivate()
    {
        PowerupsManager.bounceCount = 0;
    }
}