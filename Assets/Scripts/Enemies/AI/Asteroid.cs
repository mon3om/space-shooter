using System.Collections;
using UnityEngine;

public class Asteroid : EnemyAIBase
{
    private bool isVisible = false;

    private void Start()
    {
        base.Start();
        float scale = Random.Range(2.5f, 5f);
        transform.localScale = new(scale, scale, 1);
        enemyDamager.health *= scale;
        orientationHandler.StartRotatingInAngle(new(0, 0, Random.Range(0, 180)));
        directionalMover.MoveInDirection((Vector3)enteringTargetPosition - transform.position);
        directionalMover.movementSpeed = Random.Range(1f, 2.5f);

        StartCoroutine(DeathCoroutine());
    }

    public void OnBecameVisible()
    {
        isVisible = true;
    }

    public void OnBecameInvisible()
    {
        isVisible = false;
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(20);
        if (!isVisible)
        {
            if (TryGetComponent(out ScreenEdgeAlert screenEdgeAlert))
                screenEdgeAlert.DestroyAlert();
            DestroyEnemy();
        }
        else
            StartCoroutine(DeathCoroutine());
    }
}