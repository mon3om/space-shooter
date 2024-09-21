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

        StartCoroutine(DeathCoroutine(20));
    }

    public void OnBecameVisible()
    {
        isVisible = true;
    }

    public new void OnBecameInvisible()
    {
        isVisible = false;
        StopAllCoroutines();
        StartCoroutine(DeathCoroutine(3));
    }

    private IEnumerator DeathCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (!isVisible)
        {
            if (TryGetComponent(out ScreenEdgeAlert screenEdgeAlert))
                screenEdgeAlert.DestroyAlert();
            DestroyEnemy();
        }
        else
            StartCoroutine(DeathCoroutine(time));
    }
}