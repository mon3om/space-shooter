namespace Powerups
{
    // Increase your ship's movement speed, enhancing maneuverability.
    public class ThrusterUpgrade : PowerupBase
    {
        protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
        {
            playerInstance.GetComponent<PlayerMovement>().speedModifier += 0.1f;
        }

        protected override void CustomDeactivate()
        {

        }
    }
}