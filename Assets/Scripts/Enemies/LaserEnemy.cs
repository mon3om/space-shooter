using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : EnemyAIBase
{
    public GameObject laserPrefab, laserCollisionEffect;
    public float laserPositionOffset;

    private GameObject laserInstance, effectInstance = null;

    private Vector2 laserDirection = Vector2.left;

    void Start()
    {
        base.Start();
        laserDirection = transform.up;
        StartShootingLaser();
    }

    void Update()
    {

        var obstaclePosition = FindObstaclePosition();

        if (obstaclePosition.x != Mathf.Infinity)
        {
            if (effectInstance == null)
                effectInstance = Instantiate(laserCollisionEffect);
            if (effectInstance && !effectInstance.activeInHierarchy)
                effectInstance.SetActive(true);

            var laserRenderer = laserInstance.GetComponent<SpriteRenderer>();
            laserRenderer.size = new(laserRenderer.size.x, Vector2.Distance(transform.position, obstaclePosition) / 3f);
            laserInstance.transform.position = transform.position + Vector3.left * ((laserRenderer.bounds.size.x / 2f) + laserPositionOffset);
            effectInstance.transform.position = obstaclePosition;
        }
        else
        {
            if (effectInstance)
                effectInstance.SetActive(false);

            var laserRenderer = laserInstance.GetComponent<SpriteRenderer>();
            laserRenderer.size = new(laserRenderer.size.x, 5);
            laserInstance.transform.position = transform.position + transform.up * ((laserRenderer.bounds.size.x / 2f) + laserPositionOffset);
        }
    }

    private void StartShootingLaser()
    {
        if (!laserInstance)
        {
            laserInstance = Instantiate(laserPrefab, transform.position, transform.rotation, transform);
            laserInstance.transform.localScale /= transform.localScale.x;
        }

        var laserRenderer = laserInstance.GetComponent<SpriteRenderer>();
        laserRenderer.size = new(laserRenderer.size.x, 5);
        laserInstance.transform.position = transform.position + transform.up * ((laserRenderer.bounds.size.x / 2f) + laserPositionOffset);
    }

    private void StopShootingLaser()
    {

    }

    private Vector2 FindObstaclePosition()
    {
        var hit = Physics2D.Raycast((Vector2)transform.position + laserDirection, laserDirection, CameraUtils.CameraRect.xMax * 2);
        if (hit && hit.transform && hit.transform != transform)
        {
            return hit.point;
        }

        return Vector2.positiveInfinity;
    }

    private void OnDestroy()
    {
        Destroy(laserInstance);
        Destroy(effectInstance);
    }
}
