using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wave.Helper;

enum LaserBossState { Laser, Shooting, Travelling }

public class LaserBoss : EnemyAIBase
{
    private LaserBossState laserBossState = LaserBossState.Travelling;
    [SerializeField] private float shootingInterval = 5f;
    [SerializeField] private float laserInterval = 5f;
    private bool combineLaserAndShooting = false;

    private void Start()
    {
        // components
        base.Start();
        shootingBases = GetComponentsInChildren<ShootingBase>();
        movementPositions = new(){new(-CameraUtils.CameraRect.xMax + 1,CameraUtils.CameraRect.yMax-1),
        new(0,CameraUtils.CameraRect.yMax-1),new(CameraUtils.CameraRect.xMax - 1,CameraUtils.CameraRect.yMax-1)
        };

        InstantiateLasers();
        orientationHandler.LookAtPointImmediate(enteringTargetPosition);
        directionalMover.MoveTowardsPoint(enteringTargetPosition);
        directionalMover.onDestinationReached.AddListener(OnStartLaser);
        StartCoroutine(ManagerCoroutine());
    }

    private void FixedUpdate()
    {
        HandleLaserLength();
    }

    private IEnumerator ManagerCoroutine()
    {
        StartShooting();
        yield return new WaitForSeconds(shootingInterval / 3f);
        SpawnEnemies();
        yield return new WaitForSeconds(shootingInterval / 3f);
        SpawnEnemies();
        yield return new WaitForSeconds(shootingInterval / 3f);
        StartLaserAttack();
        yield return new WaitForSeconds(laserInterval / 3f);
        SpawnEnemies();
        yield return new WaitForSeconds(laserInterval / 3f);
        SpawnEnemies();
        yield return new WaitForSeconds(laserInterval / 3f);
        StartCoroutine(ManagerCoroutine());
    }

    #region Shooting
    private ShootingBase[] shootingBases;
    private int counter = 0;

    private void StartShooting()
    {
        OnStartShooting();
        StopLaser();
        StartMoving();
        orientationHandler.StopRotating();
        orientationHandler.StartRotatingTowardsTransform(player);
    }

    private void OnStartShooting()
    {
        laserBossState = LaserBossState.Shooting;
        foreach (var item in shootingBases)
        {
            item.GetComponent<OrientationHandler>().StartRotatingTowardsTransform(player);
        }
        StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator ShootingCoroutine()
    {
        if (counter % 2 == 0)
            shootingBases[0].Fire(shootingBases[0].gameObject, Vector3.up);
        else
            shootingBases[1].Fire(shootingBases[1].gameObject, Vector3.up);

        yield return new WaitForSeconds(3);
        counter++;
        if (laserBossState == LaserBossState.Shooting || combineLaserAndShooting)
            StartCoroutine(ShootingCoroutine());
    }

    private void StopShooting()
    {
        StopCoroutine(ShootingCoroutine());
    }
    #endregion

    #region Laser
    [Space]
    [Header("Laser")]
    public GameObject laserPrefab;
    public float laserDistanceFromEdges = 1f;
    private List<Transform> lasers = new List<Transform>();
    private void StartLaserAttack()
    {
        directionalMover.onDestinationReached.RemoveAllListeners();
        directionalMover.onDestinationReached.AddListener(OnStartLaser);
        directionalMover.MoveTowardsPoint(new(0, 0, 0));
        orientationHandler.StartRotatingInAngle(new(0, 0, 1));
        // StopShooting();
        StopMoving();
    }

    private void OnStartLaser()
    {
        InstantiateLasers();
        laserBossState = LaserBossState.Laser;
        directionalMover.StopMoving();
        foreach (var item in lasers)
            item.gameObject.SetActive(true);
    }

    private void InstantiateLasers()
    {
        if (lasers.Count > 0) return;
        for (int i = 0; i < 4; i++)
        {
            var go = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, 45 + 90 * i), transform);
            lasers.Add(go.transform);
            go.name = "" + i;
            go.transform.position += go.transform.up * go.transform.localScale.y / 2f;
            go.SetActive(false);
            go.TryGetComponent(out SpriteRenderer spriteRenderer);
            spriteRenderer.size = new(spriteRenderer.size.x, 0);
        }
    }

    private void HandleLaserLength()
    {
        if (laserBossState != LaserBossState.Laser) return;

        for (int i = 0; i < 4; i++)
        {
            var go = lasers[i];
            LineIntersection2D.GetIntersectionWithScreenEdge(transform.position, (go.transform.position - transform.position).normalized * 30, out var intersection, out var screenEdge);
            go.TryGetComponent(out SpriteRenderer spriteRenderer);
            float distance = Vector2.Distance(transform.position, intersection) / go.transform.localScale.y;
            distance -= laserDistanceFromEdges / go.transform.localScale.y;
            distance = Mathf.Min(spriteRenderer.size.y + Time.deltaTime, distance);
            go.transform.position = go.transform.up * go.transform.localScale.y / 2f * distance;
            spriteRenderer.size = new Vector2(spriteRenderer.size.x, distance);
            go.GetComponent<BoxCollider2D>().size = spriteRenderer.size;
        }
    }

    private void StopLaser()
    {
        foreach (var item in lasers)
        {
            item.gameObject.SetActive(false);
            item.TryGetComponent(out SpriteRenderer spriteRenderer);
            spriteRenderer.size = new(spriteRenderer.size.x, 0);
        }
    }
    #endregion

    #region Movement
    [Space]
    [Header("Movement")]
    [SerializeField] private float movementInterval = 15;
    private int positionsCounter = 0;
    private List<Vector2> movementPositions;
    private bool isMoving = false;

    private void StartMoving()
    {
        StartCoroutine(MovingCoroutine());
        isMoving = true;
    }

    private IEnumerator MovingCoroutine()
    {
        directionalMover.MoveTowardsPoint(movementPositions[positionsCounter]);
        directionalMover.onDestinationReached.RemoveAllListeners();
        directionalMover.onDestinationReached.AddListener(() =>
        {
            directionalMover.StopMoving();
        });
        yield return new WaitForSeconds(movementInterval);

        positionsCounter++;
        if (positionsCounter >= movementPositions.Count) positionsCounter = 0;
        if (isMoving)
            StartCoroutine(MovingCoroutine());
    }

    private void StopMoving()
    {
        StopCoroutine(MovingCoroutine());
        isMoving = false;
    }
    #endregion

    private void SpawnEnemies()
    {
        var pickedEnemy = WavesManager.WaveEnemies.Where(x => x.enemyPrefab.name.Contains("Kamikaze")).ToList()[0];
        var spawned = EnemyFactory.SpawnNoWave(pickedEnemy, WavesManager.Instance.transform);
        PositionManager.SetPositions(spawned, pickedEnemy);
    }
}
