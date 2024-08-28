using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JsonWaves;
using UnityEngine;
using Wave.Helper;

enum LaserBossState { Laser, Shooting, Travelling }

public class LaserBoss : EnemyAIBase
{
    private LaserBossState laserBossState = LaserBossState.Travelling;
    [SerializeField] private float shootingInterval = 5f;
    [SerializeField] private float laserInterval = 5f;
    private bool combineLaserAndShooting = false;

    private List<Vector2> screenPositions;
    [SerializeField] private ShootingSettings doubleCanons, singleCanon;
    private List<Transform> spawnedEnemies = new();

    private void Start()
    {
        // components
        base.Start();
        shootingBases = GetComponentsInChildren<ShootingBase>();

        screenPositions = Utils.DivideScreen(5, 4);

        orientationHandler.LookAtPointImmediate(enteringTargetPosition);
        directionalMover.MoveTowardsPoint(enteringTargetPosition);
        directionalMover.onDestinationReached.AddListener(OnStartLaser);
        StartCoroutine(ManagerCoroutine());

        foreach (var item in shootingBases)
            item.shootingSettings = singleCanon;
        enemyDamager.onDamageTaken += OnDamageTaken;
    }

    private void FixedUpdate()
    {
        HandleLaserLength();
    }

    private void OnDamageTaken(DamageData damageData)
    {
        if (damageData.currentHealth <= damageData.initialHealth / 2f)
        {
            foreach (var item in shootingBases)
                item.shootingSettings = doubleCanons;
        }

        if (damageData.currentHealth <= 0)
            foreach (var item in spawnedEnemies)
                if (item)
                    Destroy(item.gameObject);
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
        if (shootingBases[0].shootingSettings == singleCanon)
            yield return ShootSingleCanon();
        else
            yield return ShootDoubleCanon();
    }
    private IEnumerator ShootSingleCanon()
    {
        if (counter % 2 == 0)
            shootingBases[0].Fire(shootingBases[0].gameObject, Vector3.up);
        else
            shootingBases[1].Fire(shootingBases[1].gameObject, Vector3.up);

        yield return new WaitForSeconds(shootingBases[0].shootingSettings.burstCount * shootingBases[0].shootingSettings.burstDelay + 3f);
        counter++;
        if (laserBossState == LaserBossState.Shooting || combineLaserAndShooting)
            StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator ShootDoubleCanon()
    {
        shootingBases[0].Fire(shootingBases[0].gameObject, Vector3.up);
        shootingBases[1].Fire(shootingBases[1].gameObject, Vector3.up);

        yield return new WaitForSeconds(shootingBases[0].shootingSettings.burstCount * shootingBases[0].shootingSettings.burstDelay + 3f);
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
        StopShooting();
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
            var go = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            lasers.Add(go.transform);
            go.name = "" + i;
        }
    }

    private void HandleLaserLength()
    {
        if (laserBossState != LaserBossState.Laser) return;

        for (int i = 0; i < 4; i++)
        {
            var go = lasers[i];
            Vector3 intersectionVector;
            if (i == 0)
                intersectionVector = (transform.up - transform.position).normalized * 30;
            else if (i == 1)
                intersectionVector = (transform.right - transform.position).normalized * 30;
            else if (i == 2)
                intersectionVector = (-transform.right - transform.position).normalized * 30;
            else
                intersectionVector = (-transform.up - transform.position).normalized * 30;

            LineIntersection2D.GetIntersectionWithScreenEdge(transform.position, intersectionVector, out var intersection, out var screenEdge);
            go.TryGetComponent(out LaserPrefab _laser);
            _laser.StartPoint = transform.position;
            _laser.EndPoint = intersection;
        }
    }

    private void StopLaser()
    {
        foreach (var item in lasers)
        {
            item.TryGetComponent(out LaserPrefab _laser);
            _laser.isEmitting = false;
        }

        lasers.Clear();
    }
    #endregion

    #region Movement
    [Space]
    [Header("Movement")]
    [SerializeField] private float movementInterval = 15;
    private bool isMoving = false;

    private void StartMoving()
    {
        StartCoroutine(MovingCoroutine());
        isMoving = true;
    }

    private IEnumerator MovingCoroutine()
    {
        directionalMover.MoveTowardsPoint(screenPositions[Random.Range(0, screenPositions.Count)]);
        directionalMover.onDestinationReached.RemoveAllListeners();
        directionalMover.onDestinationReached.AddListener(() =>
        {
            directionalMover.StopMoving();
        });
        yield return new WaitForSeconds(movementInterval);

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
        var pickedEnemy = JsonWavesManager.GetWaves().Where(x => x.enemyPrefab.name.Contains("Kamikaze")).ToList()[0];
        var spawned = EnemyFactory.SpawnNoWave(pickedEnemy, null);
        PositionManager.SetPositions(spawned, pickedEnemy);
        spawnedEnemies = spawned;
    }
}
