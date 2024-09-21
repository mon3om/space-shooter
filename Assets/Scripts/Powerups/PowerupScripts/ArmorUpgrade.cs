namespace Powerups
{
    // Permanently increase your ship's maximum health for better durability.
    public class ArmorUpgrade : PowerupBase
    {
        protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
        {
            playerInstance.GetComponent<PlayerDamager>().EditInitHealth(1);
        }

        protected override void CustomDeactivate()
        {

        }
    }
}