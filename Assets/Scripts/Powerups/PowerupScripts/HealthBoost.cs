using UnityEngine;

namespace Powerups
{
    public class HealthBoost : PowerupBase
    {
        protected override void CustomActivate(PowerupScriptableObject powerupScriptableObject)
        {
            playerInstance.GetComponent<PlayerDamager>().EditHealth(1);
        }

        protected override void CustomDeactivate()
        {

        }
    }
}