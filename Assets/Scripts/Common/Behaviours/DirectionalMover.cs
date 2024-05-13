using UnityEngine;
using UnityEngine.Events;

public enum MovementType { TowardsTransform, TowardsPoint, Directional }
public class DirectionalMover : MonoBehaviour
{
    public float movementSpeed = 1;
    public bool stopWhenDirectionReached = true;
    public bool physicsMovement = false;
    public MovementType movementType;

    //
    [HideInInspector] public UnityEvent onDirectionReached;

    private Vector3 targetPoint;
    private Transform targetTransform;
    private bool isMoving = false;

    private Rigidbody2D rb;

    private void Start()
    {
        TryGetComponent(out rb);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void StartMovingTowardsPoint(Vector3 point)
    {
        isMoving = true;
        targetPoint = point;
        movementType = MovementType.TowardsPoint;
    }

    public void StartMovingTowardsDirection(Vector3 direction)
    {
        isMoving = true;
        targetPoint = direction;
        movementType = MovementType.Directional;
    }

    public void StartMovingTowardsTransform(Transform target)
    {
        isMoving = true;
        targetTransform = target;
        movementType = MovementType.TowardsTransform;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private void Move()
    {
        if (!isMoving) return;

        Vector3 destination;
        switch (movementType)
        {
            case MovementType.TowardsPoint:
                destination = (targetPoint - transform.position).normalized;
                break;
            case MovementType.TowardsTransform:
                destination = targetTransform.position;
                break;
            case MovementType.Directional:
                destination = targetPoint;
                break;
            default:
                throw new System.Exception("Unhandled");
        }

        if (physicsMovement)
            rb.MovePosition(transform.position + (destination - transform.position).normalized * movementSpeed * Time.fixedDeltaTime);
        else
            transform.position += movementSpeed * Time.fixedDeltaTime * destination.normalized;

        if (movementType == MovementType.TowardsPoint && stopWhenDirectionReached && IsDestinationReached())
        {
            StopMoving();
            onDirectionReached?.Invoke();
        }
    }

    private bool IsDestinationReached()
    {
        return Vector2.Distance(targetPoint, transform.position) <= 0.05f;
    }
}