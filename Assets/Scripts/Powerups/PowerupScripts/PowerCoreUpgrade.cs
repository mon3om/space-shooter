using UnityEngine;

namespace Powerups
{
    public class PowerCoreUpgrade : PowerupBase
    {
        protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
        {
            PowerupsManager.powerCoreModifier += 0.1f;
        }

        protected override void CustomDeactivate()
        {

        }
    }
}