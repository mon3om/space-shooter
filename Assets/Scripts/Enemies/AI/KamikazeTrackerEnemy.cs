using System.Collections;
using UnityEngine;

public class KamikazeTrackerEnemy : EnemyAIBase
{
    public float trackingSpeedMultiplier = 1.1f;
    public float rotationSpeedDivider = 10;
    [Tooltip("Used to speed up the enemy until reaching max speed")]
    public float speedFactor = 0.02f;
    public float explosionDamage = 10f;
    public float waitTimeBeforeTrackingPlayer = 0;

    private bool isSpeedingUp = false;
    private float initSpeed;
    private GameObject targetTransform;

    private new IEnumerator Start()
    {
        if (CheckIfVerticalPositionOccupied())
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(Start());
            yield break;
        }

        base.Start();
        directionalMover.MoveTowardsPoint(enteringTargetPosition);
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        orientationHandler.LookAtPointImmediate(enteringTargetPosition);
        initSpeed = directionalMover.movementSpeed;

        targetTransform = new("temp");
        targetTransform.transform.parent = transform;
        targetTransform.transform.position = transform.position + transform.up;
    }

    private void FixedUpdate()
    {
        if (isSpeedingUp)
            if (directionalMover.movementSpeed < initSpeed * trackingSpeedMultiplier)
                directionalMover.movementSpeed += Time.fixedDeltaTime * speedFactor;
    }

    private void OnDestinationReached()
    {
        isNowStatic = true;
        directionalMover.StopMoving();
        StartCoroutine(StartKamikazeCoroutine());
    }

    private IEnumerator StartKamikazeCoroutine()
    {
        orientationHandler.StartRotatingTowardsTransform(Instances.Player);

        yield return new WaitForSeconds(Random.Range(0f, 2f));

        yield return new WaitForSeconds(waitTimeBeforeTrackingPlayer);
        isNowStatic = false;
        isSpeedingUp = true;

        orientationHandler.rotationSpeed /= rotationSpeedDivider;
        directionalMover.MoveTowardsTransform(targetTransform.transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            if (other.gameObject.TryGetComponent(out PlayerDamager playerDamager))
            {
                playerDamager.TakeDamage(explosionDamage);
            }
            else
            {
                Debug.LogError("PlayerDamager not found");
            }
            InstantiateDeathAnimation();
        }
    }
}
