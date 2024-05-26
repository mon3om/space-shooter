using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float health = 10;
    public System.Action<DamageData> onDamageTaken; // (damage, currentHealth, initialHealth)

    private float initialHealth;

    private void Start()
    {
        initialHealth = health;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        DamageData damageData = new(amount, health, initialHealth);
        onDamageTaken?.Invoke(damageData);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.ENEMY_BULLET)) return;

        if (other.transform.TryGetComponent(out Projectile projectile))
        {
            TakeDamage(projectile.projectileData.damage);
            if (health <= 0)
            {
                if (TryGetComponent<EnemyAIBase>(out var componentAI))
                {
                    componentAI.InstantiateDeathAnimation();
                }

                Destroy(gameObject);
            }
            else
            {
                if (projectile.projectileData.explosionPrefab != null)
                    Instantiate(projectile.projectileData.explosionPrefab, other.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                else
                    Debug.LogWarning("ExplosionPrefab is null");
            }
            Destroy(other.gameObject);
        }
    }
}

public class DamageData
{
    public float damage;
    public float currentHealth;
    public float initialHealth;

    public DamageData(float damage, float currentHealth, float initialHealth)
    {
        this.damage = damage;
        this.currentHealth = currentHealth;
        this.initialHealth = initialHealth;
    }
}
