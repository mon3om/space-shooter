using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : EnemyAIBase
{
    public ShootingSettings shootingSettings;

    // Components
    private ShootingBase shootingBase;
    [Space]
    public float shootingTime;
    public float minesXCount, minesYCount;
    public float minYPos;
    public float minesSpawnDelay;
    public float minesRotationSpeed;
    public GameObject mineAlert;
    public GameObject minePrefab;

    private bool isShooting = false;
    private EnhancedHoverer enhancedHoverer;

    private List<GameObject> spawnedMines = new();

    private void Start()
    {
        base.Start();
        GetComponent<EnemyPowerupDropper>().powerupsCount = PowerupsManager.bossPowerupsCount;
        enhancedHoverer = GetComponent<EnhancedHoverer>();
        enhancedHoverer.SetHovering(false);
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;
        orientationHandler.LookAtPointImmediate(enteringTargetPosition);
        directionalMover.MoveTowardsPoint(enteringTargetPosition);

        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        onEnemyDestroy += OnEnemyDestroy;
    }

    private void OnDestinationReached()
    {
        orientationHandler.StartRotatingTowardsTransform(player);
        directionalMover.onDestinationReached.RemoveAllListeners();
        directionalMover.StopMoving();

        enhancedHoverer.SetHovering(true);
        isShooting = true;
        StartCoroutine(ShootingCoroutine());
        StartCoroutine(StopShootingCoroutine());
    }

    private void OnEnemyDestroy(EnemyAIBase enemyAI, int waveId)
    {
        ClearMines();
    }

    private IEnumerator ShootingCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        if (!isShooting) yield break;
        shootingBase.Fire(gameObject, Vector3.up);
        yield return new WaitForSeconds(shootingBase.shootingSettings.fireRate);
        if (isShooting) StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator StopShootingCoroutine()
    {
        yield return new WaitForSeconds(shootingTime);
        isShooting = false;
        StopCoroutine(ShootingCoroutine());
        enhancedHoverer.SetHovering(false);
        directionalMover.MoveTowardsPoint(new(0, 0));
        directionalMover.onDestinationReached.RemoveAllListeners();
        directionalMover.onDestinationReached.AddListener(() =>
        {
            directionalMover.StopMoving();
            StartCoroutine(MinesCoroutine());
        });
    }

    private IEnumerator MinesCoroutine()
    {
        var locations = Utils.DivideScreen(minesXCount, minesYCount, minYPos);

        foreach (var item in locations)
        {
            bool minePositionOccupied = false;
            foreach (var mine in spawnedMines)
            {
                if (mine && (Vector2)mine.transform.position == item)
                {
                    minePositionOccupied = true;
                    break;
                }
            }
            if (!minePositionOccupied)
            {
                orientationHandler.StartRotatingTowardsPoint(item, minesRotationSpeed);
                yield return new WaitForSeconds(minesSpawnDelay);
                ShootMine(item);
            }
        }

        yield return new WaitForSeconds(2);
        OnDestinationReached();
    }

    private void ShootMine(Vector2 location)
    {
        var go = Instantiate(minePrefab, transform.position, Quaternion.identity);
        go.TryGetComponent(out Mine mine);
        mine.destination = location;
        mine.OnDestroy += () => spawnedMines.Remove(mine.gameObject);
        spawnedMines.Add(go);
    }

    private void ClearMines()
    {
        for (int i = spawnedMines.Count - 1; i >= 0; i--)
        {
            if (spawnedMines[i] != null)
                Destroy(spawnedMines[i]);

            spawnedMines.RemoveAt(i);
        }
    }
}