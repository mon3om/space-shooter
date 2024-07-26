using UnityEngine;

[RequireComponent(typeof(EnemyDamager))]
public class PowerupDropper : MonoBehaviour
{
    public GameObject powerupPrefab;

    private EnemyDamager enemyDamager;

    void Start()
    {
        enemyDamager = GetComponent<EnemyDamager>();
        enemyDamager.onDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(DamageData damageData)
    {
        if (damageData.currentHealth <= 0)
        {
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }
    }
}
