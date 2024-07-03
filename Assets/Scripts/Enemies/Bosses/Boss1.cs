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
    public float minesRandomVariation;
    public float minesSpawnDelay;
    public GameObject mineAlert;
    public GameObject minePrefab;

    private bool isShooting = false;
    private float minesRotationSpeed;

    private void Start()
    {
        base.Start();
        if (TryGetComponent(out EnhancedHoverer hoverer)) hoverer.SetHovering(false);
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;
        orientationHandler.LookAtPointImmediate(enteringTargetPosition);
        directionalMover.MoveTowardsPoint(enteringTargetPosition);
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);

        minesRotationSpeed = orientationHandler.rotationSpeed * 2;
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
        var locations = GetMinesLocations();

        foreach (var item in locations)
        {
            orientationHandler.StartRotatingTowardsPoint(item, minesRotationSpeed);
            // Instantiate(mineAlert, item, Quaternion.identity);
            yield return new WaitForSeconds(minesSpawnDelay);
            FireMine(item);
        }

        yield return new WaitForSeconds(2);
        OnDestinationReached();
    }

    private List<Vector2> GetMinesLocations()
    {
        List<Vector2> locations = new();
        for (float x = CameraUtils.CameraRect.xMin + 1; x < CameraUtils.CameraRect.xMax - 1; x += CameraUtils.CameraRect.xMax * 2 / minesXCount)
            for (float y = CameraUtils.CameraRect.yMin + 1; y < CameraUtils.CameraRect.yMax - 1; y += CameraUtils.CameraRect.yMax * 2 / minesYCount)
                locations.Add(new Vector2(x + Random.Range(-minesRandomVariation, minesRandomVariation), y + Random.Range(-minesRandomVariation, minesRandomVariation)));

        return locations;
    }

    private void FireMine(Vector2 location)
    {
        var go = Instantiate(minePrefab, transform.position, Quaternion.identity);
        go.TryGetComponent(out Mine mine);
        mine.destination = location;
    }
}