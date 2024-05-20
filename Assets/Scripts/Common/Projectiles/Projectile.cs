using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;

    private SpriteRenderer spriteRenderer;
    ProjectileMovement projectileMovement;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectileMovement = GetComponent<ProjectileMovement>();

        transform.localScale = projectileData.scale;
        if (projectileData.sprite != null) spriteRenderer.sprite = projectileData.sprite;
        projectileMovement.speed = projectileData.speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (projectileData.projectileSource == ProjectileSource.Enemy)
        {
            if (other.TryGetComponent(out PlayerDamager playerDamager))
            {
                playerDamager.TakeDamage(projectileData.damage);
            }
        }
    }
}