using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSupportEnemy : EnemyAIBase
{
    public EnemyAIBase boss;
    public float distanceFromShip;
    public float regenrationPercent;

    private LineRenderer lineRenderer;
    private bool isRegenerating = false;

    void Start()
    {
        base.Start();
        directionalMover.MoveTowardsTransform(boss.transform);
        orientationHandler.StartRotatingTowardsTransform(boss.transform);
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!boss)
        {
            lineRenderer.positionCount = 0;
            return;
        }
        if (Vector2.Distance(transform.position, boss.transform.position) <= distanceFromShip)
            directionalMover.StopMoving();

        if (!directionalMover.isMoving)
        {
            if (Vector2.Distance(transform.position, boss.transform.position) > distanceFromShip + 1)
                transform.position += (boss.transform.position - transform.position).normalized * directionalMover.movementSpeed * Time.deltaTime;
            else if (Vector2.Distance(transform.position, boss.transform.position) < distanceFromShip - 1)
                transform.position += (transform.position - boss.transform.position).normalized * directionalMover.movementSpeed * Time.deltaTime;

            lineRenderer.SetPositions(new Vector3[] { transform.position, boss.transform.position });

            if (!isRegenerating)
            {
                isRegenerating = true;
                StartCoroutine(RegenerateCoroutine());
            }
        }

    }

    private IEnumerator RegenerateCoroutine()
    {
        boss.GetComponent<Shield>().Regenerate(regenrationPercent);
        yield return new WaitForSeconds(1);
        StartCoroutine(RegenerateCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
