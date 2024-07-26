using UnityEngine;

public class LaserPrefab : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    public float emissionSpeed = 3f;

    [HideInInspector] public Vector2 start;
    [HideInInspector] public Vector2 end;
    [HideInInspector] public Transform source;
    [HideInInspector] public bool isEmitting = true;

    private Vector2 initEnd;

    private static float HEIGHT_DIVIDER;

    void Start()
    {
        HEIGHT_DIVIDER = transform.localScale.y;
        initEnd = end;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = new(spriteRenderer.size.x, 0);

        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isEmitting)
            Emit();
        else
            StopEmitting();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform != source && other.TryGetComponent<LaserObject>(out var laserObject))
        {
            if (!isEmitting) return;
            laserObject.AddLaserSource(this);
            end = other.ClosestPoint(start);
            laserObject.InstantiateDeflectorSources();
        }

        if (other.CompareTag(Tags.PLAYER_SHIP))
            other.GetComponent<PlayerDamager>().TakeDamage(1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform != source && other.TryGetComponent<LaserObject>(out var laserObject))
        {
            laserObject.RemoveLaserSource(this);
            end = initEnd;
        }
    }

    private void Emit()
    {
        spriteRenderer.size = new(spriteRenderer.size.x, Mathf.Min(spriteRenderer.size.y + Time.deltaTime * emissionSpeed, Vector2.Distance(start, end) / HEIGHT_DIVIDER));
        transform.position = start + (initEnd - start).normalized * HEIGHT_DIVIDER * spriteRenderer.size.y / 2f;
        transform.rotation = Utils.GetLookAtRotation(start, initEnd);
        boxCollider2D.size = new(boxCollider2D.size.x, spriteRenderer.size.y);
    }

    private void StopEmitting()
    {
        spriteRenderer.size = new(spriteRenderer.size.x, Mathf.Max(spriteRenderer.size.y - Time.deltaTime * emissionSpeed * 2, 0));
        transform.position = end + (start - initEnd).normalized * HEIGHT_DIVIDER * spriteRenderer.size.y / 2f;
        transform.rotation = Utils.GetLookAtRotation(initEnd, start);
        boxCollider2D.size = new(boxCollider2D.size.x, spriteRenderer.size.y);

        if (spriteRenderer.size.y <= 0.05f)
            Destroy(gameObject);
    }
}
