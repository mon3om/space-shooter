using UnityEngine;

public enum ShootingType { Bullets = 0 }
public enum ShootingMode { Single, Burst }
public abstract class ShootingBase : MonoBehaviour
{
    public ShootingType shootingType = ShootingType.Bullets;
    public ShootingSettings shootingSettings;
    public abstract void Fire(GameObject origin, Vector3 direction);
}