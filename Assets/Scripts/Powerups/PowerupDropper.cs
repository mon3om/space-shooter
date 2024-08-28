using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyDamager))]
public class PowerupDropper : MonoBehaviour
{
    public GameObject powerupPrefab;
    public int powerupsCount = 1;

    private EnemyDamager enemyDamager;
    private List<PowerupScriptableObject> instantiatedPowerups = new();
    private int iterations = 0;

    void Start()
    {
        enemyDamager = GetComponent<EnemyDamager>();
        enemyDamager.onDamageTaken += OnDamageTaken;
    }

    private void OnDamageTaken(DamageData damageData)
    {
        if (damageData.currentHealth <= 0)
        {
            float angleStep = 360f / powerupsCount;
            for (int i = 0; i < powerupsCount; i++)
            {
                float angle = i * angleStep;
                float radians = angle * Mathf.Deg2Rad;

                float x = Mathf.Cos(radians) * Mathf.Clamp((powerupsCount - 1) * 0.5f, 0, 2.5f);
                float y = Mathf.Sin(radians) * Mathf.Clamp((powerupsCount - 1) * 0.5f, 0, 2.5f);
                Vector3 position = new Vector3(x, y, 0f);
                var go = Instantiate(powerupPrefab, transform.position, Quaternion.identity);
                go.GetComponent<DirectionalMover>().MoveTowardsPoint(position);
                PickPowerup(go);
            }

            foreach (var el in instantiatedPowerups)
            {
                el.OnPowerupActivated += () =>
                    {
                        LevelManager.Instance.OnBossDefeated();
                        foreach (var item in GameObject.FindGameObjectsWithTag(Tags.POWERUP))
                            Destroy(item);
                    };
            }
        }
    }

    private void PickPowerup(GameObject powerup)
    {
        if (++iterations > 50)
        {
            Debug.Log("Error picking powerup, 50 iterations passed with no success");
            return;
        }

        powerup.TryGetComponent<DroppablePowerup>(out var droppablePowerup);
        PowerupScriptableObject powerupSO = droppablePowerup.PickRandomPowerup(instantiatedPowerups);

        // Check if the powerup requires other powerups to be equipped and change it if the condition is not met
        if (powerupSO.requiredPowerups.Length > 0)
        {
            bool conditionMet = false;
            foreach (var equipped in PowerupsManager.equippedPowerups)
            {
                foreach (var required in powerupSO.requiredPowerups)
                {
                    if (equipped.id == required.id)
                    {
                        conditionMet = true;
                        break;
                    }
                }
                if (conditionMet)
                    break;
            }

            if (!conditionMet)
            {
                Debug.Log("Condition not met -- retrying");
                PickPowerup(powerup);
                return;
            }
            Debug.Log("Condition met!");
        }

        instantiatedPowerups.Add(powerupSO);
    }


}
