using System.Collections;
using UnityEngine;

public class ShootingAssaultEnemy : EnemyAIBase
{
    public ShootingSettings shootingSettings;
    public Vector3 destination;

    // Components
    private ShootingBase shootingBase;

    private void Start()
    {
        base.Start();
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;

        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        directionalMover.MoveTowardsPoint(destination);
        orientationHandler.LookAtPointImmediate(destination);
    }

    private void OnDestinationReached()
    {
        directionalMover.StopMoving();
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
