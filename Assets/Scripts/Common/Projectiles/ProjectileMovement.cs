using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10;

    private Vector3 direction = Vector3.zero;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    public void SetMovementDirection(Vector3 direction)
    {
        this.direction = direction;
        transform.localRotation = Utils.GetLookAtRotation(direction);
    }

    private void FixedUpdate()
    {
        if (direction != Vector3.zero)
        {
            transform.position = transform.position + speed * Time.fixedDeltaTime * direction;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyDamager enemyDamager = other.gameObject.GetComponent<EnemyDamager>();
            enemyDamager.TakeDamage(5);
            Destroy(gameObject);
        }
    }
}