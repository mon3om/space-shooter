using System.Collections;
using UnityEngine;

public class ShootingAssaultEnemy : EnemyAIBase
{
    public ShootingSettings shootingSettings;
    [HideInInspector] public Vector3 destination;

    // Components
    private ShootingBase shootingBase;

    private void Start()
    {
        base.Start();
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;

        if (destination.x > 0)
            transform.position = new(CameraUtils.CameraRect.width / 2 + 2, transform.position.y, transform.position.z);
        else
            transform.position = new(-CameraUtils.CameraRect.width / 2 + 2, transform.position.y, transform.position.z);

        directionalMover.onDirectionReached.AddListener(OnDestinationReached);
        directionalMover.StartMovingTowardsPoint(destination);
        orientationHandler.LookAtPointImmediate(destination);
    }

    private void OnDestinationReached()
    {
        orientationHandler.StartRotatingTowardsTransform(Instances.Player);
        StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator ShootingCoroutine()
    {
        yield return new WaitForSeconds(shootingBase.shootingSettings.fireRate);

        shootingBase.Fire(gameObject, Vector3.up);

        StartCoroutine(ShootingCoroutine());
    }
}
