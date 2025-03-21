using UnityEngine;

[ExecuteInEditMode]
public class LaserPrefab : MonoBehaviour
{
    [SerializeField] private GameObject collisionEffect;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    public float emissionSpeed = 5f;
    public float damage = 1;

    public Vector2 StartPoint;
    public Vector2 EndPoint { get; private set; }
    [HideInInspector] public Transform source;
    [HideInInspector] public bool isEmitting = true;

    private static float HEIGHT_DIVIDER;
    private Transform effectInstance;
    private bool defaultEndPointDistance = true;


    void Start()
    {
        HEIGHT_DIVIDER = transform.localScale.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = new(spriteRenderer.size.x, 0);
        boxCollider2D = GetComponent<BoxCollider2D>();


        if (defaultEndPointDistance)
        {
            EndPoint = StartPoint + (EndPoint - StartPoint).normalized * 30;
        }
        effectInstance = Instantiate(collisionEffect, transform).transform;

        if (!source)
        {
            Debug.LogError("No source, destroying laser");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!source)
        {
            Debug.LogWarning("No source, destroying laser");
            Destroy(gameObject);
        }

        if (isEmitting)
            Emit();
        else
            StopEmitting();

        effectInstance.position = transform.position + ((Vector3)EndPoint - transform.position).normalized * spriteRenderer.size.y * HEIGHT_DIVIDER / 2;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform != source && other.TryGetComponent<LaserObject>(out var laserObject))
        {
            if (source && laserObject.enemyIdentifier.waveId != source.GetComponent<EnemyAIBase>().enemyIdentifier.waveId) return;
            if (!isEmitting) return;
            laserObject.AddLaserSource(this);
            EndPoint = other.ClosestPoint(StartPoint);
            laserObject.InstantiateDeflectorSources();
        }

        if (other.CompareTag(Tags.PLAYER_SHIP))
            other.GetComponent<PlayerDamager>().TakeDamage(damage);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform != source && other.TryGetComponent<LaserObject>(out var laserObject))
        {
            laserObject.RemoveLaserSource(this);
            EndPoint = StartPoint + (EndPoint - StartPoint).normalized * 30;
        }
    }

    public void SetEndpoint(Vector2 endPoint, bool defaultEndPointDistance = true)
    {
        EndPoint = endPoint;
        this.defaultEndPointDistance = defaultEndPointDistance;
    }

    private void Emit()
    {
        spriteRenderer.size = new(spriteRenderer.size.x, Mathf.Min(spriteRenderer.size.y + Time.deltaTime * emissionSpeed, Vector2.Distance(StartPoint, EndPoint) / HEIGHT_DIVIDER));
        transform.position = StartPoint + (EndPoint - StartPoint).normalized * HEIGHT_DIVIDER * spriteRenderer.size.y / 2f;
        transform.rotation = Utils.GetLookAtRotation(StartPoint, EndPoint);
        boxCollider2D.size = new(boxCollider2D.size.x, spriteRenderer.size.y);
    }

    private void StopEmitting()
    {
        Destroy(effectInstance.gameObject);
        Destroy(gameObject);
    }
}
