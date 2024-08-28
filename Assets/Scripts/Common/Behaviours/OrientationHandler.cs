using UnityEngine;

public class OrientationHandler : MonoBehaviour
{
    public float angleOffset = 0;
    public float rotationSpeed = 1;

    [Space]
    public bool lookAtPlayer = false;

    private bool isRotating = false;
    private bool isRotatingInAngle = false;
    private Vector3 rotationAngleEuleur;
    private bool isTargetTransform = true;
    private Transform targetTransform = null;
    private Vector2 targetPoint;
    private float previousRotationSpeed;

    private float initialRotation;

    public System.Action OnRotationReached;

    private void Awake()
    {
        previousRotationSpeed = rotationSpeed;
    }

    private void Start()
    {
        if (lookAtPlayer)
        {
            targetTransform = GameObject.FindWithTag(Tags.PLAYER_SHIP).transform;
            StartRotatingTowardsTransform(targetTransform);
        }
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            if (isRotatingInAngle)
            {
                transform.Rotate(rotationAngleEuleur * Time.fixedDeltaTime * rotationSpeed);
            }
            else
            {
                LookAtTransformOrPoint();
            }
        }
    }

    public void StartRotatingTowardsTransform(Transform target, float speed = -1, bool temporaryNewSpeed = true)
    {
        SetRotationSpeed(speed, temporaryNewSpeed);

        isRotatingInAngle = false;
        targetTransform = target;
        isRotating = true;
        isTargetTransform = true;
    }

    public void StartRotatingTowardsPoint(Vector2 point, float speed = -1, bool temporaryNewSpeed = true)
    {
        SetRotationSpeed(speed, temporaryNewSpeed);

        isRotatingInAngle = false;
        targetPoint = point;
        isRotating = true;
        isTargetTransform = false;

        initialRotation = transform.rotation.z;
    }

    public void StartRotatingInAngle(Vector3 euleur, float speed = -1, bool temporaryNewSpeed = true)
    {
        SetRotationSpeed(speed, temporaryNewSpeed);

        isRotating = true;
        isRotatingInAngle = true;
        rotationAngleEuleur = euleur;
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    private void LookAtTransformOrPoint()
    {
        Vector3 target;
        if (isTargetTransform) // Look at transform
        {
            if (!targetTransform)
                return;
            target = targetTransform.position;
        }
        else // Rotate towards point
        {
            target = targetPoint;

            // Check if rotation is reached here
        }

        target.z = 0f;

        Vector3 objectPos = transform.position;
        target.x -= objectPos.x;
        target.y -= objectPos.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleOffset));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void LookAtPointImmediate(Vector2 point)
    {
        isRotatingInAngle = false;
        Vector3 target = point;
        target.z = 0f;

        Vector3 objectPos = transform.position;
        target.x -= objectPos.x;
        target.y -= objectPos.y;

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleOffset));
        transform.rotation = rotation;
    }

    private void SetRotationSpeed(float speed, bool temporaryNewSpeed)
    {
        if (speed != -1)
        {
            if (temporaryNewSpeed)
                previousRotationSpeed = rotationSpeed;
            else
                previousRotationSpeed = speed;

            rotationSpeed = speed;
        }
        else
        {
            rotationSpeed = previousRotationSpeed;
        }
    }

    private bool HasReachedRotation()
    {
        return default;
    }
}