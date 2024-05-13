using UnityEngine;

public class TrackerEnemy : EnemyAIBase
{
    private void Start()
    {
        base.Start();

        Transform targetTransform = new GameObject().transform;
        targetTransform.parent = transform;
        targetTransform.localPosition = Vector3.up;

        directionalMover.StartMovingTowardsTransform(targetTransform);
        orientationHandler.StartRotatingTowardsTransform(Instances.Player);
    }
}
