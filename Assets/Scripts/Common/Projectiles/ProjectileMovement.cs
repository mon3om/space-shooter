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
        if (useVar1)
        {
            Var1();
        }
        else
        {
            if (direction != Vector3.zero)
            {
                transform.position = transform.position + speed * Time.fixedDeltaTime * direction;
            }
        }
    }

    [Space]
    [Header("Var1")]
    public bool useVar1 = false;
    public float var1Speed;
    public float var1Variation;
    private bool direct = false;
    private float counter = 0;
    private void Var1()
    {
        transform.position = transform.position + speed * Time.fixedDeltaTime * direction;

        transform.position += transform.right * var1Speed * (direct ? 1 : -1) * Time.fixedDeltaTime;
        counter += speed * 2 * (direct ? 1 : -1) * Time.fixedDeltaTime;
        if (Mathf.Abs(counter) >= var1Variation)
        {
            counter = 0;
            direct = !direct;
        }
    }
}