using System.Collections.Generic;
using UnityEngine;


public class LaserObject : EnemyAIBase
{
    public Vector2 enterDirection;
    public List<Vector2> emittingDirections = new();
    public bool isSource = false;
    public float totalTravelTime = 5;


    [Space]
    public GameObject laserPrefab;
    public GameObject laserEffect;

    private Vector3 initPosition;
    private bool isEmitting = false;
    private List<LaserPrefab> lasers = new();
    private List<LaserPrefab> laserSources = new();

    private new void Start()
    {
        base.Start();

        initPosition = transform.position;
        if (isSource)
            orientationHandler.LookAtPointImmediate((Vector2)transform.position + emittingDirections[0]);

        UpdateInitPosition();
        enteringTargetPosition = initPosition;
        directionalMover.MoveTowardsPoint(enteringTargetPosition);
        directionalMover.movementSpeed = GetTravellingSpeed();
        directionalMover.onDestinationReached.AddListener(() =>
        {
            directionalMover.StopMoving();
            if (isSource)
                CreateLaser(transform.up);
        });

        onEnemyDestroy += (EnemyAIBase ai, int waveId) =>
        {
            StopEmitting();
            Debug.Log("Lasers = " + lasers.Count);
        };
    }

    private void UpdateInitPosition()
    {
        if (enterDirection == Vector2.left)
            transform.position = new(CameraUtils.CameraRect.xMin - 3, transform.position.y);
        else if (enterDirection == Vector2.right)
            transform.position = new(CameraUtils.CameraRect.xMax + 3, transform.position.y);
        else if (enterDirection == Vector2.up)
            transform.position = new(transform.position.x, CameraUtils.CameraRect.yMax + 3);
        else if (enterDirection == Vector2.down)
            transform.position = new(transform.position.x, CameraUtils.CameraRect.yMin - 3);
    }

    private float GetTravellingSpeed()
    {
        return Vector2.Distance(transform.position, initPosition) / totalTravelTime;
    }

    public void StopEmitting()
    {
        isEmitting = false;
        foreach (var item in lasers)
            item.isEmitting = isEmitting;
    }

    public void InstantiateDeflectorSources()
    {
        if (isSource) return;
        if (isEmitting) return;
        isEmitting = true;

        if (emittingDirections.Contains(Vector2.left))
            CreateLaser(Vector3.left);
        if (emittingDirections.Contains(Vector2.right))
            CreateLaser(Vector3.right);
        if (emittingDirections.Contains(Vector2.up))
            CreateLaser(Vector3.up);
        if (emittingDirections.Contains(Vector2.down))
            CreateLaser(Vector3.down);
    }

    private void CreateLaser(Vector3 direction)
    {
        var _laserPrefab = Instantiate(laserPrefab).GetComponent<LaserPrefab>();
        _laserPrefab.start = transform.position;
        _laserPrefab.end = transform.position + 20 * direction;
        _laserPrefab.source = transform;

        lasers.Add(_laserPrefab);
    }

    public void AddLaserSource(LaserPrefab source)
    {
        if (laserSources.Contains(source)) return;
        laserSources.Add(source);
    }

    public void RemoveLaserSource(LaserPrefab source)
    {
        if (!laserSources.Contains(source)) return;
        laserSources.Remove(source);
        if (laserSources.Count == 0)
            StopEmitting();
    }
}