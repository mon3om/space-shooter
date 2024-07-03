using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float health = 10;
    public System.Action<DamageData> onDamageTaken; // (damage, currentHealth, initialHealth)

    private float initialHealth;
    private EnemyAIBase enemyAIBase;

    [Space]
    private Shield shield;

    private void Start()
    {
        enemyAIBase = GetComponent<EnemyAIBase>();
        initialHealth = health;
        shield = transform.GetComponentInChildren<Shield>();
    }

    public void TakeDamage(Projectile projectile)
    {
        // Return if shield exists and took the damage
        if (shield && shield.TakeDamageIfShieldActive(projectile.shootingSettings.damage)) return;

        health -= projectile.shootingSettings.damage;
        DamageData damageData = new(projectile.shootingSettings.damage, health, initialHealth);
        onDamageTaken?.Invoke(damageData);

        if (health <= 0)
        {
            enemyAIBase.InstantiateDeathAnimation();
            enemyAIBase.DestroyEnemy();

            if (enemyAIBase.enemyIdentifier.waveEnemyDifficulty == WaveEnemyDifficulty.Easy) UIScoreManager.UpdateScore(10);
            else if (enemyAIBase.enemyIdentifier.waveEnemyDifficulty == WaveEnemyDifficulty.Moderate) UIScoreManager.UpdateScore(15);
            else if (enemyAIBase.enemyIdentifier.waveEnemyDifficulty == WaveEnemyDifficulty.Hard) UIScoreManager.UpdateScore(50);
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
