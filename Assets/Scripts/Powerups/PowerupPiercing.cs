
public class PowerupPiercing : PowerupBase
{
    protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
    {
        PowerupsManager.piercingCount++;
    }

    protected override void CustomDeactivate()
    {
        PowerupsManager.piercingCount = 0;
    }
}