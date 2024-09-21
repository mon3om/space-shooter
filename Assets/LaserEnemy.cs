using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LaserEnemy : EnemyAIBase
{
    // State variable is used to indicate how the enemies will behave
    // 0 for single enemy
    // 1 for corner enemies with vertical laser
    // 2 for corner enemies with inclined laser
    public int state;
    public GameObject laserPrefab;

    private LaserPrefab laserInstance;
    private bool isModifying = true;
    private float modifiedAmount = 0;
    private bool isIncreasing = false;

    private int id;
    void Start()
    {
        base.Start();
        id = GetId();
        PickRandomState();

        if (state != 0) // State 1 and 2
        {
            if (id == 0)
                enteringTargetPosition = CameraUtils.TopLeft;
            if (id == 1)
                enteringTargetPosition = CameraUtils.TopRight;

            orientationHandler.LookAtPointImmediate(enteringTargetPosition);
            directionalMover.MoveTowardsPoint(enteringTargetPosition);
            if (state == 1)
                directionalMover.onDestinationReached.AddListener(OnDestinationReachedState1);
            else
                directionalMover.onDestinationReached.AddListener(OnDestinationReachedState2);
        }
        else
        {
            bool rightPosition = Random.value > 0.5f;
            enteringTargetPosition = rightPosition ? CameraUtils.TopRight : CameraUtils.TopLeft;
            directionalMover.MoveTowardsPoint(enteringTargetPosition);
            orientationHandler.LookAtPointImmediate((Vector2)transform.position + (rightPosition ? Vector2.left : Vector2.right));
            directionalMover.onDestinationReached.AddListener(OnDestinationReachedState0);
        }
    }

    private void Update()
    {
        if (state == 0 && laserInstance)
        {
            laserInstance.StartPoint = new(laserInstance.StartPoint.x, transform.position.y);
            laserInstance.EndPoint = new(laserInstance.EndPoint.x, transform.position.y);
        }
        if (laserInstance)
        {
            ModifyLaserLength();
        }
    }

    private void OnDestinationReachedState1()
    {
        orientationHandler.StartRotatingTowardsPoint(transform.position + Vector3.down);

        InstantiateLaser();
        laserInstance.StartPoint = transform.position;
        laserInstance.EndPoint = transform.position - Vector3.up * (CameraUtils.CameraRect.yMax + transform.position.y);
        laserInstance.emissionSpeed /= 2;
    }

    private void OnDestinationReachedState2()
    {
        Vector2 targetPoint;
        if (id == 1)
            targetPoint = CameraUtils.BottomLeft;
        else
            targetPoint = CameraUtils.BottomRight;

        targetPoint += (targetPoint - (Vector2)transform.position).normalized * 0.8f + Vector2.down * 0.2f;
        orientationHandler.StartRotatingTowardsPoint(targetPoint);

        InstantiateLaser();
        laserInstance.emissionSpeed /= 2;
        laserInstance.StartPoint = transform.position;
        laserInstance.SetEndpoint(targetPoint);
    }

    private void OnDestinationReachedState0()
    {
        InstantiateLaser();
        laserInstance.StartPoint = transform.position;
        laserInstance.SetEndpoint(transform.position + Vector3.left * Mathf.Sign(transform.position.x) * (CameraUtils.CameraRect.xMax + Mathf.Abs(transform.position.x)));
        if (directionalMover.movementSpeed > 1)
            directionalMover.movementSpeed /= 4;

        if (transform.position.y > 0)
            directionalMover.MoveTowardsPoint(new(transform.position.x, CameraUtils.BottomRight.y));
        else
            directionalMover.MoveTowardsPoint(new(transform.position.x, CameraUtils.TopRight.y));
    }

    private void PickRandomState()
    {
        if (id == 0)
        {
            if (waveSiblings.Count > 1)
            {
                int _state = Random.value > 0.5f ? 2 : 2;
                foreach (var item in waveSiblings)
                    (item as LaserEnemy).state = _state;
            }
            else
                state = 0;
        }
    }

    private void InstantiateLaser()
    {
        if (laserInstance) return;

        var go = Instantiate(laserPrefab);
        go.TryGetComponent(out laserInstance);
        onEnemyDestroy += (EnemyAIBase enemyAIBase, int waveId) => laserInstance.isEmitting = false;
        laserInstance.damage = damage;
        laserInstance.source = transform;
    }

    private void ModifyLaserLength()
    {
        if (isModifying)
        {
            if (isIncreasing)
            {
                Vector2 end = laserInstance.EndPoint + (laserInstance.EndPoint - (Vector2)transform.position).normalized * Time.deltaTime;
                laserInstance.EndPoint = end;
            }
            else
            {
                Vector2 end = laserInstance.EndPoint + ((Vector2)transform.position - laserInstance.EndPoint).normalized * Time.deltaTime;
                laserInstance.EndPoint = end;
            }

            modifiedAmount += Time.deltaTime;
            if (modifiedAmount >= 2)
            {
                modifiedAmount = 2;
                isModifying = false;
                isIncreasing = !isIncreasing;
            }
        }
        else
        {
            modifiedAmount -= Time.deltaTime;
            if (modifiedAmount <= 0)
            {
                modifiedAmount = 0;
                isModifying = true;
            }
        }
    }
}
