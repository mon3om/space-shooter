using System.Collections;
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

    private void Awake()
    {
        initPosition = transform.position;
    }

    private void Start()
    {
        base.Start();
        if (emittingDirections.Count > 0)
            orientationHandler.LookAtPointImmediate((Vector2)transform.position + emittingDirections[0]);

        UpdateInitPosition();
        StartMoving();

        onEnemyDestroy += (EnemyAIBase ai, int waveId) =>
        {
            if (transform.parent && transform.parent.childCount <= 1)
                Destroy(transform.parent.gameObject);
            StopEmitting();
        };
    }

    private void UpdateInitPosition()
    {
        if (enterDirection == Vector2.left)
            transform.position = new(CameraUtils.CameraRect.xMin - 3, initPosition.y);
        else if (enterDirection == Vector2.right)
            transform.position = new(CameraUtils.CameraRect.xMax + 3, initPosition.y);
        else if (enterDirection == Vector2.up)
            transform.position = new(initPosition.x, CameraUtils.CameraRect.yMax + 3);
        else if (enterDirection == Vector2.down)
            transform.position = new(initPosition.x, CameraUtils.CameraRect.yMin - 3);
    }

    private void StartMoving()
    {
        enteringTargetPosition = initPosition;
        directionalMover.MoveTowardsPoint(enteringTargetPosition);
        directionalMover.movementSpeed = GetTravellingSpeed();
        directionalMover.onDestinationReached.AddListener(() =>
        {
            directionalMover.StopMoving();
            if (isSource)
                CreateLaser(transform.up);
        });

        if (emittingDirections.Count > 1)
        {
            for (int i = 1; i < emittingDirections.Count; i++)
            {
                GameObject go = new("Deflector");
                go.transform.position = transform.position;
                go.transform.localScale = transform.localScale;
                go.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                go.AddComponent<OrientationHandler>();
                go.TryGetComponent(out OrientationHandler _oHandler);
                _oHandler.angleOffset = -90;
                _oHandler.LookAtPointImmediate(go.transform.position + (Vector3)emittingDirections[i]);
                go.transform.parent = transform;
            }
        }
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

        foreach (var item in emittingDirections)
            CreateLaser(item);
    }

    private void CreateLaser(Vector2 direction)
    {
        var _laserPrefab = Instantiate(laserPrefab).GetComponent<LaserPrefab>();
        _laserPrefab.StartPoint = transform.position;
        _laserPrefab.SetEndpoint((Vector2)transform.position + direction);
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