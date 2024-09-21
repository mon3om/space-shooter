using UnityEngine;

[CreateAssetMenu(fileName = "ShootingSettings", menuName = "ShootingSettings")]
public class ShootingSettings : ScriptableObject
{
    public float fireRate;
    public int shotsCount;

    [Space]
    [SerializeField] private float angleBetweenShots;
    public bool randomAngle = false;
    public RandomFloat randomAnglesVariation;
    [Space]

    public ShootingMode shootingMode = ShootingMode.Single;
    public GameObject bulletPrefab;
    [Space]

    public int burstCount;
    public float burstDelay;
    [Space]

    public float shotMovementSpeed = -1;
    public float damage;
    public Vector2 scale;
    public ProjectileSource projectileSource;
    public ShootingType shootingType;
    public Sprite sprite;
    public GameObject[] explosionPrefab;
    public AudioClip soundEffect;

    public float AngleBetweenShots
    {
        set { angleBetweenShots = value; }
        get
        {
            if (randomAngle) return randomAnglesVariation.value;
            else return angleBetweenShots;
        }
    }
}

public enum ProjectileSource { Player, Enemy }
public enum ShootingType { Bullet = 0, HomingMissile, Laser }
public enum ShootingMode { Single, Burst }