using UnityEngine;

public class PeacefulEnemy : EnemyAIBase
{
    public Vector3 direction = Vector3.down;

    private void Start()
    {
        base.Start();

        directionalMover.StartMovingTowardsDirection(direction);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
