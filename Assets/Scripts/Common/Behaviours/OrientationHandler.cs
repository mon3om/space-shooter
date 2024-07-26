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
                LookAtTransform();
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

    private void LookAtTransform()
    {
        Vector3 targ;
        if (isTargetTransform)
        {
            if (!targetTransform)
                return;
            targ = targetTransform.position;
        }
        else
            targ = targetPoint;

        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleOffset));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void LookAtPointImmediate(Vector2 point)
    {
        isRotatingInAngle = false;
        Vector3 targ = point;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
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
}