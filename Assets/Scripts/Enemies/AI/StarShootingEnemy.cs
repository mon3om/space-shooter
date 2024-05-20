using System.Collections;
using UnityEngine;

public class StarShootingEnemy : EnemyAIBase
{
    public float cycleDuration = 5f;
    public RandomFloat verticalMovementSteps = new RandomFloat(2, 4);
    [HideInInspector] public Vector3 stoppingPoint;

    private ShootingBase shootingBase;

    private void Start()
    {
        base.Start();
        shootingBase = GetComponent<ShootingBase>();
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        MoveToNextPoint();
    }

    private void OnDestinationReached()
    {
        StartCoroutine(DestinationReachedCoroutine());
    }

    private Vector3 GetNextStoppingPoint()
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(-CameraUtils.CameraRect.size.x / 2f - 1, CameraUtils.CameraRect.size.x / 2f - 1);
        pos.y -= Random.Range(1f, CameraUtils.CameraRect.size.y / verticalMovementSteps.value);

        return pos;
    }

    private void MoveToNextPoint()
    {
        Vector3 targetPoint = GetNextStoppingPoint();
        directionalMover.MoveTowardsPoint(targetPoint);
        orientationHandler.StartRotatingTowardsPoint(targetPoint);
    }

    private IEnumerator DestinationReachedCoroutine()
    {
        directionalMover.StopMoving();
        orientationHandler.StartRotatingTowardsPoint(transform.position + Vector3.down);

        yield return new WaitForSeconds(cycleDuration / 2f);

        EmitStars();

        yield return new WaitForSeconds(cycleDuration / 2f);

        MoveToNextPoint();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void EmitStars()
    {
        for (int i = 0; i < shootingBase.shootingSettings.shotsCount; i++)
        {
            var go = Instantiate(shootingBase.shootingSettings.bulletPrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity);
            ProjectileMovement projectileMovement = go.GetComponent<ProjectileMovement>();
            go.transform.RotateAround(transform.position, Vector3.forward, i * 360f / shootingBase.shootingSettings.shotsCount);
            projectileMovement.SetMovementDirection((go.transform.position - transform.position).normalized);
            go.tag = Tags.ENEMY_BULLET;
        }
    }
}