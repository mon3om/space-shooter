using UnityEngine;

public abstract class PowerupBase : MonoBehaviour
{
    public static System.Action<PowerupBase> OnPowerupReady;
    [HideInInspector] public Transform playerInstance;

    private void Start()
    {
        OnPowerupReady?.Invoke(this);
    }

    public PowerupBase Activate()
    {
        if (!playerInstance) OnPowerupReady?.Invoke(this);
        BaseActivate();
        return this;
    }
    public PowerupBase Deactivate()
    {
        if (!playerInstance) OnPowerupReady?.Invoke(this);
        BaseDeactivate();
        return this;
    }

    protected abstract void CustomActivate();
    protected abstract void CustomDeactivate();

    private void BaseActivate()
    {
        CustomActivate();
    }
    private void BaseDeactivate()
    {
        CustomDeactivate();
    }
}
