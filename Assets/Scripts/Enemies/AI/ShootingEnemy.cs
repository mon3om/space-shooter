using System.Collections;
using System.Linq;
using UnityEngine;
using Wave.Helper;

public class ShootingEnemy : EnemyAIBase
{
    public ShootingSettings shootingSettings;

    // Components
    private ShootingBase shootingBase;

    private IEnumerator Start()
    {
        base.Start();
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;

        directionalMover.onDestinationReached.AddListener(OnDestinationReached);

        yield return new WaitForEndOfFrame();
        SetAllAssaultEnemiesPositions();
    }

    public void MoveTowardsDestination(Vector3 destination)
    {
        directionalMover.StopMoving();
        directionalMover.MoveTowardsPoint(destination);
        orientationHandler.LookAtPointImmediate(destination);
    }

    private void OnDestinationReached()
    {
        directionalMover.StopMoving();
        orientationHandler.StartRotatingTowardsTransform(Instances.Player);
        StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator ShootingCoroutine()
    {
        yield return new WaitForSeconds(shootingBase.shootingSettings.fireRate);
        shootingBase.Fire(gameObject, Vector3.up);

        StartCoroutine(ShootingCoroutine());
    }

    private void SetAllAssaultEnemiesPositions()
    {
        if (name.Contains("0"))
        {
            var siblings = WavesManager.Instance.GetEnemiesByTypeAndWaveId<ShootingEnemy>(waveId);
            var positions = new PositionManager().GetAssaultEnemiesPositions(siblings.Select(el => el.transform).ToList());
            if (positions.Count != siblings.Count)
                Debug.LogError("Error");
            for (int i = 0; i < positions.Count; i++)
            {
                (siblings[i] as ShootingEnemy).MoveTowardsDestination(positions[i]);
            }
        }
    }
}
