using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float health = 10;
    public System.Action<DamageData> onDamageTaken; // (damage, currentHealth, initialHealth)

    public float initialHealth { get; private set; }
    private EnemyAIBase enemyAIBase;

    [Space]
    private EnemyShield shield;

    private void Start()
    {
        enemyAIBase = GetComponent<EnemyAIBase>();
        initialHealth = health;
        shield = transform.GetComponentInChildren<EnemyShield>();
    }

    public void TakeDamage(Projectile projectile)
    {
        DamageData damageData = new(projectile.shootingSettings.damage, health, initialHealth);
        damageData.damage *= PowerupsManager.powerCoreModifier; // Apply damage bonuses

        // Return if shield exists and took the damage
        if (shield && shield.TakeDamageIfShieldActive(damageData.damage)) return;

        health -= damageData.damage;
        onDamageTaken?.Invoke(damageData);
        StartCoroutine(SpriteBlinking());

        if (health <= 0)
        {
            if (enemyAIBase.enemyIdentifier.waveEnemyDifficulty == WaveEnemyDifficulty.Easy) UIScoreManager.UpdateScore(10);
            else if (enemyAIBase.enemyIdentifier.waveEnemyDifficulty == WaveEnemyDifficulty.Moderate) UIScoreManager.UpdateScore(15);
            else if (enemyAIBase.enemyIdentifier.waveEnemyDifficulty == WaveEnemyDifficulty.Hard) UIScoreManager.UpdateScore(50);

            enemyAIBase.InstantiateDeathAnimation();
            enemyAIBase.killedByPlayer = true;
            enemyAIBase.DestroyEnemy();
        }
    }

    private IEnumerator SpriteBlinking()
    {
        enemyAIBase.Blink(true, Color.white);
        yield return new WaitForEndOfFrame();
        enemyAIBase.Blink(false);
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
