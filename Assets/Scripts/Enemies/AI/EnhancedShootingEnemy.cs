using System.Collections;
using System.Linq;
using UnityEngine;

public class EnhancedShootingEnemy : EnemyAIBase
{
    public ShootingSettings shootingSettings;

    // Components
    private ShootingBase shootingBase;

    private void Start()
    {
        base.Start();
        if (TryGetComponent(out Hoverer hoverer)) hoverer.enabled = false;
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;
        orientationHandler.LookAtPointImmediate(enteringTargetPosition);
        directionalMover.MoveTowardsPoint(enteringTargetPosition);
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);

        HandleSiblingsHoverer();
    }

    private void OnDestinationReached()
    {
        directionalMover.StopMoving();
        StartCoroutine(ShootingCoroutine());
        if (TryGetComponent(out Hoverer hoverer)) hoverer.enabled = true;
    }

    private IEnumerator ShootingCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        shootingBase.Fire(gameObject, Vector3.up);
        yield return new WaitForSeconds(shootingBase.shootingSettings.fireRate);
        StartCoroutine(ShootingCoroutine());
    }

    private void HandleSiblingsHoverer()
    {
        if (waveSiblings.Count > 0 && GetId() == 0)
        {
            waveSiblings = waveSiblings.OrderBy(_ => Random.Range(0f, 1f)).ToList();

            for (int i = 0; i < waveSiblings.Count; i++)
            {
                if (waveSiblings[i].TryGetComponent<Hoverer>(out var hoverer))
                {
                    hoverer.xOffset += Random.Range(0.5f, 2f) * i;
                    hoverer.yOffset += Random.Range(0.5f, 2f) * i;
                }
            }
        }
    }
}