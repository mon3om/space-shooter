using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public ShootingSettings shootingSettings;
    [HideInInspector] public int id;

    protected SpriteRenderer spriteRenderer;
    protected DirectionalMover directionalMover;
    protected OrientationHandler orientationHandler;
    protected Animator animator;

    private bool isOutsideScreen = false;

    public System.Action onProjectileDestroy;

    private int piercingCounter = 0;

    protected void Start()
    {
        TryGetComponent(out directionalMover);
        TryGetComponent(out orientationHandler);
        TryGetComponent(out animator);
        TryGetComponent(out spriteRenderer);


        // For regular shots
        // TO-DO this should implement (DirectionalMover) instead of (ProjectileMovement)
        // (ProjectileMovement) is deprecated and should be removed
        if (TryGetComponent(out ProjectileMovement projectileMovement))
        {
            projectileMovement.speed = shootingSettings.shotMovementSpeed;

            transform.localScale = shootingSettings.scale;
            if (shootingSettings.sprite != null) spriteRenderer.sprite = shootingSettings.sprite;
        }

        StartCoroutine(DestroyCoroutine(20));
    }

    private IEnumerator DestroyCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyProjectile();
    }

    private void OnBecameInvisible()
    {
        isOutsideScreen = true;
        if (gameObject.activeInHierarchy)
            StartCoroutine(DestroyCoroutine(5));
    }

    private void OnBecameVisible()
    {
        isOutsideScreen = false;
        StopCoroutine(DestroyCoroutine(5));
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (isOutsideScreen) return;

        if (shootingSettings.projectileSource == ProjectileSource.Enemy)
        {
            if (other.gameObject.TryGetComponent(out PlayerDamager playerDamager))
            {
                playerDamager.TakeDamage(shootingSettings.damage);
                DestroyProjectile();
            }
        }

        if (shootingSettings.projectileSource == ProjectileSource.Player)
        {
            if (other.transform.TryGetComponent(out EnemyDamager enemyDamager))
            {
                enemyDamager.TakeDamage(this);

                // Screen shake
                CameraShaker.Shake(0.08f, 0.1f);

                // Explosion effect
                foreach (var item in shootingSettings.explosionPrefab)
                    if (item)
                        Destroy(Instantiate(item, transform.position,
                        // Instantiate rotation
                        item.transform.rotation.eulerAngles == Vector3.zero ? Quaternion.Euler(0, 0, Random.Range(0, 360)) : item.transform.rotation
                        ),
                        // Destroy time
                        5);

                DestroyProjectile();
                piercingCounter++;
            }
        }
    }

    public void DestroyProjectile()
    {
        if (shootingSettings.shootingType == ShootingType.HomingMissile || piercingCounter >= PowerupsManager.piercingCount)
        {
            onProjectileDestroy?.Invoke();
            Destroy(gameObject);
        }
    }

    private void Bounce()
    {

    }
}