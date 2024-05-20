using System.Collections;
using UnityEngine;

public class PeacefulTrackerEnemy : EnemyAIBase
{
    public float trackingSpeedMultiplier = 1.1f;
    [HideInInspector] public float waitTimeBeforeTrackingPlayer = 0;
    [HideInInspector] public Vector3 stoppingPoint;

    private void Start()
    {
        base.Start();
        Vector3 targetPoint = transform.position;
        targetPoint.y = stoppingPoint.y;
        directionalMover.MoveTowardsPoint(targetPoint);

        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        orientationHandler.LookAtPointImmediate(targetPoint);
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnDestinationReached()
    {
        directionalMover.StopMoving();
        StartCoroutine(StartTrackingCoroutine());
    }

    private IEnumerator StartTrackingCoroutine()
    {
        yield return new WaitForSeconds(waitTimeBeforeTrackingPlayer);
        GetComponent<Collider2D>().isTrigger = false;
        directionalMover.movementSpeed *= trackingSpeedMultiplier;
        directionalMover.MoveTowardsTransform(Instances.Player);
        orientationHandler.StartRotatingTowardsTransform(Instances.Player);
    }
}
