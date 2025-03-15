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
        orientationHandler.StartRotatingTowardsTransform(boss.transform);
    }

    void Update()
    {
        if (!boss)
        {
            lineRenderer.positionCount = 0;
            Destroy(gameObject);
            return;
        }
        if (Vector2.Distance(transform.position, boss.transform.position) <= distanceFromShip)
        {
            directionalMover.StopMoving();
        }

        if (!directionalMover.isMoving)
        {
            RotateAroundBoss();

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
        boss.GetComponent<EnemyShield>().Regenerate(regenrationPercent);
        yield return new WaitForSeconds(1);
        StartCoroutine(RegenerateCoroutine());
    }

    private void OnBecameVisible()
    {
        GetComponent<ScreenPositionLock>().preventLeavingScreen = true;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    float angle = 0;
    private void RotateAroundBoss()
    {
        angle += 20f * Time.deltaTime;

        // Convert angle from degrees to radians
        float angleInRadians = angle * Mathf.Deg2Rad;

        // Calculate the new position using trigonometry
        float x = Mathf.Cos(angleInRadians) * 3;
        float z = Mathf.Sin(angleInRadians) * 3;

        // Update the position of the orbiting object
        transform.position = Vector3.Lerp(transform.position, new Vector3(x, z, 0) + boss.transform.position, 0.1f);
    }
}
