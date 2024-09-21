using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10;

    private Vector3 direction = Vector3.zero;

    public void SetMovementDirection(Vector3 direction)
    {
        this.direction = direction;
        transform.localRotation = Utils.GetLookAtRotation(direction);
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + speed * Time.fixedDeltaTime * direction;
    }
}