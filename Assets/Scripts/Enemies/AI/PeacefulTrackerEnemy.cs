using System.Collections;
using UnityEngine;

public class PeacefulTrackerEnemy : EnemyAIBase
{
    public float trackingSpeedMultiplier = 1.1f;
    public float explosionDamage = 10f;
    [HideInInspector] public float waitTimeBeforeTrackingPlayer = 0;
    [HideInInspector] public Vector3 stoppingPoint;

    private void Start()
    {
        base.Start();
        Vector3 targetPoint = transform.position;
        targetPoint.y = stoppingPoint.y;
        directionalMover.MoveTowardsPoint(targetPoint);

        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        orientationHandler.LookAtPointImmediate(targetPoint);
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnDestinationReached()
    {
        directionalMover.StopMoving();
        StartCoroutine(StartTrackingCoroutine());
    }

    private IEnumerator StartTrackingCoroutine()
    {
        yield return new WaitForSeconds(waitTimeBeforeTrackingPlayer);
        GetComponent<Collider2D>().isTrigger = false;
        directionalMover.movementSpeed *= trackingSpeedMultiplier;
        directionalMover.MoveTowardsTransform(Instances.Player);
        orientationHandler.StartRotatingTowardsTransform(Instances.Player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            InstantiateDeathAnimation();
            if (other.gameObject.TryGetComponent(out PlayerDamager playerDamager))
            {
                playerDamager.TakeDamage(explosionDamage);
            }
            else
            {
                Debug.LogError("PlayerDamager not found");
            }
            Destroy(gameObject);
        }
    }
}
