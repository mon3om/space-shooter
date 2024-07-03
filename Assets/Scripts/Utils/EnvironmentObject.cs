using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
    [SerializeField] private RandomFloat scaleRange;

    [HideInInspector] public float speed;

    private Vector2 destination;

    private void Start()
    {
        destination = new(Random.Range(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.xMax), -10f);
        GetComponent<SpriteRenderer>().color = new(1, 1, 1, Random.Range(0.3f, 1f));
        if (TryGetComponent(out OrientationHandler orientationHandler))
        {
            orientationHandler.rotationSpeed = Random.Range(.2f, 2f);
            orientationHandler.StartRotatingInAngle(new(0, 0, Random.Range(-2, 2) > 0 ? 1 : -1));
        }
        float scale = scaleRange.value;
        transform.localScale = new(scale, scale);
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)destination.normalized * speed * Time.fixedDeltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}