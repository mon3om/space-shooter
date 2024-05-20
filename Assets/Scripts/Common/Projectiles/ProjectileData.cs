using UnityEngine;

public enum ProjectileType
{
    SimpleBullet
}

public enum ProjectileSource
{
    Player, Enemy
}

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ProjectileData", order = 1)]
public class ProjectileData : ScriptableObject
{
    public ProjectileSource projectileSource = ProjectileSource.Player;
    public ProjectileType projectileType = ProjectileType.SimpleBullet;
    public float damage;
    public float speed;
    [Space]
    public Sprite sprite;
    public Vector2 scale;
    public GameObject explosionPrefab;
}