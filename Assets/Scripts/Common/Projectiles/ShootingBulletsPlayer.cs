using System.Collections;
using UnityEngine;

public class ShootingBulletsPlayer : ShootingBase
{
    private int burstCounter = 0;

    public override void Fire(GameObject origin, Vector3 direction)
    {
        if (shootingSettings.shootingMode == ShootingMode.Single)
            SingleShot(origin, direction);
        if (shootingSettings.shootingMode == ShootingMode.Burst)
            BurstShot(origin, direction);
    }

    public void FireVariation(GameObject origin, Vector3 direction, float variation)
    {
        if (shootingSettings.shootingMode == ShootingMode.Single)
            SingleShot(origin, direction, variation);
        if (shootingSettings.shootingMode == ShootingMode.Burst)
            BurstShot(origin, direction);
    }

    private void SingleShot(GameObject origin, Vector3 direction, float variation = 0)
    {
        for (int i = 0; i < shootingSettings.shotsCount; i++)
        {
            GameObject instantiatedBullet = Instantiate(shootingSettings.bulletPrefab, origin.transform.position, Quaternion.identity);
            ProjectileMovement ProjectileMovement = instantiatedBullet.GetComponent<ProjectileMovement>();

            if (shootingSettings.shotMovementSpeed != -1) ProjectileMovement.speed = shootingSettings.shotMovementSpeed; // Shot speed

            Vector3 movementDirection = GetMovementDirection(shootingSettings.shotsCount, i, (direction + Vector3.right * Random.Range(-variation, variation)).normalized);
            ProjectileMovement.SetMovementDirection(movementDirection);
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

        float currentAngle = startAngle + currentBulletIndex * shootingSettings.AngleBetweenShots + transform.rotation.eulerAngles.z;

        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 dir = new(direction.x < 0 ? -x : x, y, 0);

        return dir.normalized;
    }
}