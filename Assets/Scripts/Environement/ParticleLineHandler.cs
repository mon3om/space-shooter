using UnityEngine;

public class ParticleLineHandler : MonoBehaviour
{
    public Transform trackedTransform;
    private LineRenderer lineRenderer;

    [HideInInspector] public bool isActive = false;
    public static float maxLineWidth = 0.01f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0;
    }

    private void Update()
    {
        if (!isActive && lineRenderer.widthMultiplier == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (!isActive)
        {
            lineRenderer.widthMultiplier = 0;
            // lineRenderer.widthMultiplier -= Time.deltaTime / 30f;
            // lineRenderer.widthMultiplier = Mathf.Max(lineRenderer.widthMultiplier, 0);
        }
        else
        {
            lineRenderer.widthMultiplier += Time.deltaTime / 30f;
            lineRenderer.widthMultiplier = Mathf.Min(lineRenderer.widthMultiplier, maxLineWidth);
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, trackedTransform.position);
    }
}