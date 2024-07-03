using UnityEngine;

public class Asteroid : EnemyAIBase
{
    private void Start()
    {
        base.Start();
        float scale = Random.Range(2.5f, 5f);
        transform.localScale = new(scale, scale, 1);
        enemyDamager.health *= scale;
        orientationHandler.StartRotatingInAngle(new(0, 0, Random.Range(0, 180)));
        directionalMover.MoveInDirection(enteringTargetPosition - (Vector2)transform.position);
        directionalMover.movementSpeed = Random.Range(1f, 2.5f);
    }
}