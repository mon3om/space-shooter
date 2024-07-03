using System.Collections;
using UnityEngine;

public class PeacefulEnemy : EnemyAIBase
{
    private void Start()
    {
        base.Start();
        orientationHandler.LookAtPointImmediate((Vector2)transform.position + (enteringTargetPosition - (Vector2)transform.position));
        directionalMover.MoveInDirection(enteringTargetPosition - (Vector2)transform.position);
    }
}
