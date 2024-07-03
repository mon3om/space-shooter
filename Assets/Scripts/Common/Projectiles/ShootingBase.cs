using UnityEngine;
public abstract class ShootingBase : MonoBehaviour
{
    public ShootingSettings shootingSettings;
    public abstract void Fire(GameObject origin, Vector3 direction);
}