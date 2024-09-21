using UnityEngine;

namespace Powerups
{
    // Increase your ship's movement speed, enhancing maneuverability.
    public class UpdateAvailable : PowerupBase
    {
        private new void Start()
        {
            base.Start();
            if (PowerupsManager.bossPowerupsCount >= 5)
            {
                ReplacePowerup();
            }
        }

        protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
        {
            PowerupsManager.bossPowerupsCount++;
        }

        protected override void CustomDeactivate()
        {

        }
    }
}