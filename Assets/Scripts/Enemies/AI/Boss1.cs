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

    private List<GameObject> spawnedMines = new();

    private void Start()
    {
        base.Start();
        if (TryGetComponent(out EnhancedHoverer hoverer)) hoverer.SetHovering(false);
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
        if (TryGetComponent(out EnhancedHoverer hoverer)) hoverer.SetHovering(true);
        StartCoroutine(ShootingCoroutine());
        StartCoroutine(StopShootingCoroutine());
        isShooting = true;
    }

    private void OnEnemyDestroy(EnemyAIBase enemyAI, int waveId)
    {
        ClearMines();
    }

    private IEnumerator ShootingCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        shootingBase.Fire(gameObject, Vector3.up);
        yield return new WaitForSeconds(shootingBase.shootingSettings.fireRate);
        if (isShooting) StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator StopShootingCoroutine()
    {
        yield return new WaitForSeconds(shootingTime);
        if (TryGetComponent(out EnhancedHoverer hoverer)) hoverer.SetHovering(false);
        isShooting = false;
        StopCoroutine(ShootingCoroutine());
        directionalMover.onDestinationReached.AddListener(() =>
        {
            directionalMover.StopMoving();
            StartCoroutine(MinesCoroutine());
        });
        directionalMover.MoveTowardsPoint(new(0, 0));
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
        spawnedMines.Add(go);
    }

    private void ClearMines()
    {
        for (int i = 0; i < spawnedMines.Count; i++)
        {
            if (spawnedMines[i] == null) continue;
            Destroy(spawnedMines[i]);
        }
    }
}