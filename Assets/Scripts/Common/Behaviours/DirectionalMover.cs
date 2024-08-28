using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum MovementType { TowardsTransform, TowardsPoint, Directional }
public class DirectionalMover : MonoBehaviour
{
    public float movementSpeed = 1;
    public bool physicsMovement = false;
    public MovementType movementType;

    [HideInInspector] public UnityEvent onDestinationReached;
    [HideInInspector] public Transform targetTransform;
    [HideInInspector] public bool isMoving = false;
    //
    private Vector2 targetPoint;
    private Rigidbody2D rb;
    private Vector2 initPosition = Vector2.negativeInfinity;

    private void Start()
    {
        TryGetComponent(out rb);
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        if (physicsMovement)
            MoveUsedOnlyForPhysicsMovementTemporarly();
        else
        {
            switch (movementType)
            {
                case MovementType.TowardsTransform:
                    MoveToTransform();
                    break;
                case MovementType.TowardsPoint:
                    MoveToPoint();
                    break;
                case MovementType.Directional:
                    MoveInDirection();
                    break;
                default:
                    break;
            }
        }
        // IsDestinationReached
        // Checking IsDestinationReached happens inside MoveToPoint method
    }

    public void MoveTowardsPoint(Vector3 point)
    {
        isMoving = true;
        targetPoint = point;
        movementType = MovementType.TowardsPoint;
        initPosition = transform.position;
    }

    public void MoveInDirection(Vector3 direction)
    {
        isMoving = true;
        targetPoint = direction;
        movementType = MovementType.Directional;
        initPosition = transform.position;
    }

    public void MoveTowardsTransform(Transform target)
    {
        isMoving = true;
        targetTransform = target;
        movementType = MovementType.TowardsTransform;
        initPosition = transform.position;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void MoveUsedOnlyForPhysicsMovementTemporarly()
    {
        Vector3 destination;
        switch (movementType)
        {
            case MovementType.TowardsPoint:
                destination = targetPoint;
                break;
            case MovementType.TowardsTransform:
                destination = targetTransform.position;
                break;
            case MovementType.Directional:
                destination = ((Vector3)targetPoint - transform.position).normalized;
                break;
            default:
                throw new System.Exception("Unhandled");
        }

        rb.MovePosition(transform.position + (destination - transform.position).normalized * movementSpeed * Time.fixedDeltaTime);
    }

    private void MoveToTransform()
    {
        transform.position += movementSpeed * Time.fixedDeltaTime * (targetTransform.position - transform.position).normalized;
    }

    private void MoveToPoint()
    {
        var currentMove = movementSpeed * Time.fixedDeltaTime * ((Vector3)targetPoint - transform.position).normalized;
        transform.position += currentMove;

        if (IsDestinationReached(currentMove))
        {
            transform.position = targetPoint;
            StopMoving();
            onDestinationReached?.Invoke();
        }
    }

    private void MoveInDirection()
    {
        transform.position += movementSpeed * Time.fixedDeltaTime * (Vector3)targetPoint.normalized;
    }

    private bool IsDestinationReached(Vector2 currentMove)
    {
        bool goingBackwards = Vector3.Dot(currentMove, targetPoint - initPosition) < 0;
        return goingBackwards || Vector2.Distance(targetPoint, transform.position) <= 0.1f;
    }
}