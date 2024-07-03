using UnityEngine;

public class PowerupShield : PowerupBase
{
    [HideInInspector] public UIPlayerShield uIPlayerShield;

    protected override void CustomActivate()
    {
        uIPlayerShield.ChangeState(ShieldState.Charging);
    }

    protected override void CustomDeactivate()
    {
        uIPlayerShield.ChangeState(ShieldState.Disabled);
    }
}