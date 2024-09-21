using System.Collections;
using UnityEngine;
using Wave.Helper;

public class PeacefulTrackerEnemy : EnemyAIBase
{
    public float trackingSpeedMultiplier = 1.1f;
    public float explosionDamage = 10f;
    [HideInInspector] public float waitTimeBeforeTrackingPlayer = 0;
    private bool startedTracking = false;

    private IEnumerator Start()
    {
        go = new GameObject();
        go.transform.parent = transform;
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
        // GetComponent<Collider2D>().isTrigger = true;
        waitTimeBeforeTrackingPlayer = GetId();

        initialMovementSpeed = directionalMover.movementSpeed;
        initialRotationSpeed = orientationHandler.rotationSpeed;
    }

    private void OnDestinationReached()
    {
        isNowStatic = true;
        directionalMover.StopMoving();
        StartCoroutine(StartTrackingCoroutine());
    }

    private IEnumerator StartTrackingCoroutine()
    {
        isNowStatic = false;
        yield return new WaitForSeconds(waitTimeBeforeTrackingPlayer);
        startedTracking = true;
        go.transform.position = transform.position + transform.up * 5;
        directionalMover.MoveTowardsTransform(go.transform);

        StartCoroutine(CheckDirectionCoroutine());
        // GetComponent<Collider2D>().isTrigger = false;
        directionalMover.movementSpeed *= trackingSpeedMultiplier;
        orientationHandler.StartRotatingTowardsTransform(Instances.Player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PLAYER_SHIP))
        {
            other.gameObject.TryGetComponent(out PlayerDamager playerDamager);
            playerDamager.TakeDamage(explosionDamage);

            InstantiateDeathAnimation();
            DestroyEnemy();
        }
    }

    #region SlowingDown
    [Space]
    private Vector2 previousDirection;
    public float oppositeAngleOffset = 140;
    public float speedModifier = 5;
    public float variationSpeed = 1;
    private float initialMovementSpeed, initialRotationSpeed;

    GameObject go;
    private void Update()
    {
        if (!startedTracking) return;

        if (isDirectionOpposite)
        {
            if (directionalMover.movementSpeed > initialMovementSpeed / (speedModifier * 2))
                directionalMover.movementSpeed = Mathf.Lerp(directionalMover.movementSpeed, initialMovementSpeed / (speedModifier * 2), variationSpeed * Time.deltaTime);
            if (orientationHandler.rotationSpeed < initialRotationSpeed * (speedModifier / 3))
                orientationHandler.rotationSpeed = Mathf.Lerp(orientationHandler.rotationSpeed, initialRotationSpeed * (speedModifier / 3), variationSpeed * Time.deltaTime);

            if (Vector2.Angle(directionalMover.targetTransform.position - transform.position, Instances.Player.position - transform.position) < 10)
            {
                isDirectionOpposite = false;
                StartCoroutine(CheckDirectionCoroutine());
            }
        }
        else
        {

            if (directionalMover.movementSpeed < initialMovementSpeed * speedModifier)
                directionalMover.movementSpeed = Mathf.Lerp(directionalMover.movementSpeed, initialMovementSpeed * speedModifier, variationSpeed * Time.deltaTime);
            if (orientationHandler.rotationSpeed > initialRotationSpeed / speedModifier)
                orientationHandler.rotationSpeed = Mathf.Lerp(orientationHandler.rotationSpeed, initialRotationSpeed / speedModifier, variationSpeed * Time.deltaTime);
        }
    }


    private bool IsCurrentDirectionOppositeToPrevious()
    {

        Vector2 currentDirection = (directionalMover.targetTransform.position - transform.position).normalized;
        if (Vector2.Angle(currentDirection, previousDirection) >= oppositeAngleOffset)
            return true;

        return false;
    }

    public float checkInterval = 0.5f;
    private bool isDirectionOpposite = false;
    private IEnumerator CheckDirectionCoroutine()
    {
        previousDirection = (directionalMover.targetTransform.position - transform.position).normalized;

        yield return new WaitForSeconds(checkInterval);
        if (IsCurrentDirectionOppositeToPrevious())
        {
            isDirectionOpposite = true;
            yield break;
        }

        StartCoroutine(CheckDirectionCoroutine());
    }
    #endregion
}
