using System.Collections;
using UnityEngine;

public class ShootingEnemy : EnemyAIBase
{
    public ShootingSettings shootingSettings;

    // Components
    private ShootingBase shootingBase;

    private IEnumerator Start()
    {
        if (CheckIfVerticalPositionOccupied())
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(Start());
            yield break;
        }

        base.Start();
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;

        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        MoveTowardsDestination(enteringTargetPosition);
    }

    public void MoveTowardsDestination(Vector3 destination)
    {
        directionalMover.StopMoving();
        directionalMover.MoveTowardsPoint(destination);
        orientationHandler.LookAtPointImmediate(destination);
    }

    private void OnDestinationReached()
    {
        isNowStatic = true;
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
