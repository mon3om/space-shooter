using UnityEngine;

public class LaserPointMover : MonoBehaviour
{
    public float distance = 2f;
    public float speed = 1f;

    private Vector2 initPosition;
    private int initDirection;
    int direction = 1;

    private void Awake()
    {
        initPosition = transform.localPosition;
        direction *= initPosition.x > 0 ? 1 : -1;
        initDirection = direction;
    }

    private void FixedUpdate()
    {
        transform.localPosition += Vector3.right * Time.fixedDeltaTime * speed * direction;

        if (direction == initDirection)
        {
            if (Mathf.Abs(transform.localPosition.x - initPosition.x) > distance)
                direction *= -1;
        }
        else
        {
            if (Mathf.Abs(transform.localPosition.x - initPosition.x) <= 0.1f)
                direction *= -1;
        }

    }
}