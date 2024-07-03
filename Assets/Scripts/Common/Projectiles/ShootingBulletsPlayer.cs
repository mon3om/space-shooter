using System.Collections;
using UnityEngine;

public class ShootingBulletsPlayer : ShootingBase
{
    private int burstCounter = 0;
    private int torpedoCounter = 1;
    private int shotsCounter = 0;

    public override void Fire(GameObject origin, Vector3 direction)
    {
        if (shootingSettings.shootingMode == ShootingMode.Single)
            SingleShot(origin, direction);
        if (shootingSettings.shootingMode == ShootingMode.Burst)
            BurstShot(origin, direction);
    }

    public void FireVariation(GameObject origin, Vector3 direction)
    {
        if (shootingSettings.shootingMode == ShootingMode.Single)
            SingleShot(origin, direction);
        if (shootingSettings.shootingMode == ShootingMode.Burst)
            BurstShot(origin, direction);
    }

    private void SingleShot(GameObject origin, Vector3 direction)
    {
        for (int i = 0; i < shootingSettings.shotsCount; i++)
        {
            GameObject instantiatedBullet = Instantiate(shootingSettings.bulletPrefab, origin.transform.position, Quaternion.identity);
            instantiatedBullet.TryGetComponent(out Projectile projectile);
            projectile.shootingSettings = shootingSettings;
            if (shootingSettings.shootingType == ShootingType.HomingMissile)
            {
                projectile.id = shotsCounter % 2 == 0 ? -torpedoCounter : torpedoCounter;
            }

            if (instantiatedBullet.TryGetComponent(out ProjectileMovement projectileMovement))
            {
                projectileMovement.speed = shootingSettings.shotMovementSpeed; // Shot speed
                Vector3 movementDirection = GetMovementDirection(shootingSettings.shotsCount, i, direction);
                projectileMovement.SetMovementDirection(movementDirection);
            }

            shotsCounter++;
            if (shotsCounter % 2 == 0 && shotsCounter != 0)
                torpedoCounter++;

            if (shootingSettings.shotsCount > 1 && shotsCounter >= shootingSettings.shotsCount)
                Reset();

            if (shootingSettings.shotsCount == 1 && shotsCounter > shootingSettings.shotsCount)
                Reset();
        }
    }

    private void BurstShot(GameObject origin, Vector3 direction)
    {
        origin.GetComponent<MonoBehaviour>().StartCoroutine(BurstCoroutine(origin, direction));
    }

    private IEnumerator BurstCoroutine(GameObject origin, Vector3 direction)
    {
        SingleShot(origin, direction);
        burstCounter++;

        yield return new WaitForSeconds(shootingSettings.burstDelay);

        if (burstCounter < shootingSettings.burstCount)
            origin.GetComponent<MonoBehaviour>().StartCoroutine(BurstCoroutine(origin, direction));
        else
            burstCounter = 0;
    }

    private Vector3 GetMovementDirection(int totalBulletsCount, int currentBulletIndex, Vector3 direction)
    {
        float startAngle = (Vector3.Angle(Vector3.down, direction) - (totalBulletsCount - 1) * shootingSettings.AngleBetweenShots) / 2;

        float currentAngle = startAngle + currentBulletIndex * shootingSettings.AngleBetweenShots;
        if (totalBulletsCount == 1)
            currentAngle += Random.Range(-shootingSettings.AngleBetweenShots, shootingSettings.AngleBetweenShots);

        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 dir = new(direction.x < 0 ? -x : x, y, 0);

        return dir.normalized;
    }

    public void Reset()
    {
        torpedoCounter = 1;
        shotsCounter = 0;
    }
}