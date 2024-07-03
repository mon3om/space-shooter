using UnityEngine;
using UnityEngine.Events;

public enum MovementType { TowardsTransform, TowardsPoint, Directional }
public class DirectionalMover : MonoBehaviour
{
    public float movementSpeed = 1;
    public bool physicsMovement = false;
    public MovementType movementType;

    //
    [HideInInspector] public UnityEvent onDestinationReached;

    private Vector3 targetPoint;
    [HideInInspector] public Transform targetTransform;
    public bool isMoving = false;

    private Rigidbody2D rb;

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

        if (movementType == MovementType.TowardsPoint && IsDestinationReached())
        {
            onDestinationReached?.Invoke();
        }
    }

    public void MoveTowardsPoint(Vector3 point)
    {
        isMoving = true;
        targetPoint = point;
        movementType = MovementType.TowardsPoint;
    }

    public void MoveInDirection(Vector3 direction)
    {
        isMoving = true;
        targetPoint = direction;
        movementType = MovementType.Directional;
    }

    public void MoveTowardsTransform(Transform target)
    {
        isMoving = true;
        targetTransform = target;
        movementType = MovementType.TowardsTransform;
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
                destination = (targetPoint - transform.position).normalized;
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
        transform.position += movementSpeed * Time.fixedDeltaTime * (targetPoint - transform.position).normalized;
    }

    private void MoveInDirection()
    {
        transform.position += movementSpeed * Time.fixedDeltaTime * targetPoint.normalized;
    }

    private bool IsDestinationReached()
    {
        return Vector2.Distance(targetPoint, transform.position) <= 0.1f;
    }
}