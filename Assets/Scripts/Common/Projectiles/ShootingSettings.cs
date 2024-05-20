using UnityEngine;

[CreateAssetMenu(fileName = "ShootingSettings", menuName = "ShootingSettings")]
public class ShootingSettings : ScriptableObject
{
    public float fireRate;
    public int shotsCount;
    [SerializeField] private float angleBetweenShots;
    public ShootingMode shootingMode = ShootingMode.Single;
    public ShootingSource shootingSource = ShootingSource.Enemy;
    public GameObject bulletPrefab;
    [Space]
    public int burstCount;
    public float burstDelay;
    public float AngleBetweenShots
    {
        set { angleBetweenShots = value; }
        get
        {
            if (randomAngle) return randomAnglesVariation.value;
            else return angleBetweenShots;
        }
    }
    public bool randomAngle = false;
    public RandomFloat randomAnglesVariation;
    public float shotMovementSpeed = -1;
}