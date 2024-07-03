using System.Collections;
using UnityEngine;

public class SimpleShooting : MonoBehaviour
{
    public ShootingSettings shootingSettings;
    private ShootingBullets shootingBullets;

    private bool startedShooting = false;

    private EnemyAIBase enemyAIBase;

    private void Start()
    {
        shootingBullets = gameObject.AddComponent<ShootingBullets>();
        shootingBullets.shootingSettings = shootingSettings;

        TryGetComponent(out enemyAIBase);
    }

    private IEnumerator ShootingCoroutine()
    {
        if (!startedShooting && enemyAIBase)
        {
            yield return new WaitForSeconds(enemyAIBase.GetId() / 2f);
            startedShooting = true;
        }
        shootingBullets.Fire(gameObject, Vector3.up);
        yield return new WaitForSeconds(shootingSettings.fireRate);
        StartCoroutine(ShootingCoroutine());
    }

    private void OnBecameVisible()
    {
        StopAllCoroutines();
        StartCoroutine(ShootingCoroutine());
    }
}
