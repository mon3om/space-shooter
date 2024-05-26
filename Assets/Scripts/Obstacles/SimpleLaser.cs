using System.Collections;
using UnityEngine;

public class SimpleLaser : MonoBehaviour
{
    public float damage = 5f;

    private Transform child1, child2;

    private LineRenderer lineRenderer;

    private void Start()
    {
        child1 = transform.GetChild(0);
        child2 = transform.GetChild(1);
        if (child1.TryGetComponent(out EnemyDamager enemyDamager1))
            enemyDamager1.onDamageTaken += OnEmitterDamage;
        if (child2.TryGetComponent(out EnemyDamager enemyDamager2))
            enemyDamager2.onDamageTaken += OnEmitterDamage;

        StartCoroutine(CheckPlayerCollisionCoroutine());
        lineRenderer = GetComponent<LineRenderer>();
        GetComponent<DirectionalMover>().MoveInDirection(Vector3.down);
    }

    private void Update()
    {
        if (lineRenderer) lineRenderer.SetPositions(new[] { child1.position, child2.position });
    }

    private void RaycastCheck()
    {
        if (!child1 || !child2)
        {
            return;
        }

        Vector2 dir = (child2.position - child1.position).normalized;
        float dist = Vector2.Distance(child2.position, child1.position);
        RaycastHit2D hit2D = Physics2D.Raycast(child1.position, dir, dist);
        if (hit2D.collider != null)
        {
            if (hit2D.collider.gameObject.TryGetComponent(out PlayerDamager playerDamager))
                playerDamager.TakeDamage(damage);
        }
    }

    private IEnumerator CheckPlayerCollisionCoroutine()
    {
        RaycastCheck();
        yield return new WaitForSeconds(.1f);
        StartCoroutine(CheckPlayerCollisionCoroutine());
    }

    private void OnEmitterDamage(DamageData damageData)
    {
        if (damageData.currentHealth <= 0)
        {
            if (child1 && child1.TryGetComponent(out LaserPointMover laserPointMover))
            {
                Destroy(laserPointMover);
                Destroy(lineRenderer);
                Destroy(GetComponent<Animator>());
            }
            if (child2 && child2.TryGetComponent(out LaserPointMover laserPointMover1))
            {
                Destroy(laserPointMover1);
                Destroy(lineRenderer);
                Destroy(GetComponent<Animator>());
            }
        }
    }
}
