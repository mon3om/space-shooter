using UnityEngine;

public class OrientationHandler : MonoBehaviour
{
    public float angleOffset = 0;
    public float rotationSpeed = 1;

    private bool isRotating = false;
    private bool isTargetTransform = true;
    private Transform targetTransform = null;
    private Vector2 targetPoint;

    private void FixedUpdate()
    {
        if (isRotating)
            LookAtTransform();
    }

    public void StartRotatingTowardsTransform(Transform target, float speed = 1)
    {
        targetTransform = target;
        rotationSpeed = speed;
        isRotating = true;
        isTargetTransform = true;
    }

    public void StartRotatingTowardsPoint(Vector2 point, float speed = -1)
    {
        targetPoint = point;
        if (speed != -1) rotationSpeed = speed;
        isRotating = true;
        isTargetTransform = false;
    }

    public void StopRotating()
    {
        isRotating = false;
    }

    private void LookAtTransform()
    {
        Vector3 targ;
        if (isTargetTransform)
            targ = targetTransform.position;
        else
            targ = targetPoint;

        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleOffset));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void LookAtPointImmediate(Vector2 point)
    {
        Vector3 targ = point;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + angleOffset));
        transform.localRotation = rotation;
    }
}