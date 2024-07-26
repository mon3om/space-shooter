using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarShootingEnemy : EnemyAIBase
{
    public float cycleDuration = 5f;
    public RandomFloat verticalMovementSteps = new(2, 4);
    public float shootingRotationSpeed;
    [HideInInspector] public Vector3 stoppingPoint;

    // Possible screen positions
    private float xSteps = 6, ySteps = 3;
    private List<Vector2> screenPositions = new();

    private ShootingBase shootingBase;

    private void Start()
    {
        base.Start();
        shootingBase = GetComponent<ShootingBase>();
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        screenPositions = Utils.DivideScreen(xSteps, ySteps);
        MoveToNextPoint();
    }

    private void OnDestinationReached()
    {
        StartCoroutine(DestinationReachedCoroutine());
    }

    private void MoveToNextPoint()
    {
        Vector3 targetPoint = screenPositions[Random.Range(0, screenPositions.Count)];
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