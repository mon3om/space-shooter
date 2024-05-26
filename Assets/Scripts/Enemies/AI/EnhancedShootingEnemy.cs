using System.Collections;
using System.Linq;
using UnityEngine;

public class EnhancedShootingEnemy : EnemyAIBase
{
    public ShootingSettings shootingSettings;

    // Components
    private ShootingBase shootingBase;

    private void Start()
    {
        base.Start();
        shootingBase = gameObject.AddComponent<ShootingBullets>();
        shootingBase.shootingSettings = shootingSettings;
        orientationHandler.LookAtPointImmediate(Vector2.down);
        directionalMover.MoveTowardsPoint(new(transform.position.x, 2));
        directionalMover.onDestinationReached.AddListener(OnDestinationReached);
        GetComponent<Hoverer>().enabled = false;

        HandleSiblingsHoverer();
    }

    private void OnDestinationReached()
    {
        directionalMover.StopMoving();
        StartCoroutine(ShootingCoroutine());
        GetComponent<Hoverer>().enabled = true;
    }

    private IEnumerator ShootingCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        yield return new WaitForSeconds(shootingBase.shootingSettings.fireRate);
        shootingBase.Fire(gameObject, Vector3.up);
        StartCoroutine(ShootingCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Hoverer hoverer))
            hoverer.ChangeHoverSide();
    }

    private void HandleSiblingsHoverer()
    {
        if (ReferenceEquals(this, waveSiblings[0]))
        {
            waveSiblings = waveSiblings.OrderBy(_ => Random.Range(0f, 1f)).ToList();

            for (int i = 0; i < waveSiblings.Count; i++)
            {
                if (waveSiblings[i].TryGetComponent<Hoverer>(out var hoverer))
                {
                    hoverer.xOffset += Random.Range(0.5f, 2f) * i;
                    hoverer.yOffset += Random.Range(0.5f, 2f) * i;
                }
            }
        }
    }
}