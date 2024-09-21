using UnityEngine;

namespace Powerups
{
    // Increase your ship's movement speed, enhancing maneuverability.
    public class ProxyCloak : PowerupBase
    {
        protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
        {
            PowerupsManager.bossPowerupsCount++;
        }

        protected override void CustomDeactivate()
        {

        }
    }
}