using System.Collections;
using UnityEngine;

public class StarShootingEnemy : EnemyAIBase
{
    public float cycleDuration = 5f;
    public RandomFloat verticalMovementSteps = new(2, 4);
    public float shootingRotationSpeed;
    [HideInInspector] public Vector3 stoppingPoint;

    private ShootingBase shootingBase;

    private void Start()
    {
        base.Start();
        shootingBase = GetComponent<ShootingBase>();
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        MoveToNextPoint();
    }

    private void OnDestinationReached()
    {
        StartCoroutine(DestinationReachedCoroutine());
    }

    private Vector3 GetNextStoppingPoint()
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(-CameraUtils.CameraRect.size.x / 2f - 1, CameraUtils.CameraRect.size.x / 2f - 1);
        pos.y -= Random.Range(1f, CameraUtils.CameraRect.size.y / verticalMovementSteps.value);

        return pos;
    }

    private void MoveToNextPoint()
    {
        Vector3 targetPoint = GetNextStoppingPoint();
        directionalMover.MoveTowardsPoint(targetPoint);
        orientationHandler.StartRotatingTowardsPoint(targetPoint);
    }


    private IEnumerator DestinationReachedCoroutine()
    {
        directionalMover.StopMoving();
        orientationHandler.StopRotating();

        yield return new WaitForSeconds(cycleDuration / 2f);

        // Rotate and shoot
        orientationHandler.StartRotatingInAngle(Vector3.forward, shootingRotationSpeed);
        for (int i = 0; i < shootingBase.shootingSettings.burstCount; i++)
        {
            shootingBase.Fire(gameObject, Vector3.up);
            yield return new WaitForSeconds(shootingBase.shootingSettings.burstDelay);
        }
        orientationHandler.StopRotating();

        yield return new WaitForSeconds(cycleDuration / 2f);

        MoveToNextPoint();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}