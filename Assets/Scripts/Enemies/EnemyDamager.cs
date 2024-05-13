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
