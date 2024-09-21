using System.Collections;
using UnityEngine;

public class ScreenEdgeAlert : MonoBehaviour
{
    public GameObject alertPrefab;
    public bool forceAlertDisplay = false;
    public float hideDelay = 0;

    private EnemyAIBase enemyAIBase;
    private Sprite sprite;
    private float offset = 0.0f;

    private GameObject alert = null;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        if (
         transform.position.x < CameraUtils.CameraRect.xMax
         && transform.position.x > CameraUtils.CameraRect.xMin
         && transform.position.y > CameraUtils.CameraRect.yMax
         && !forceAlertDisplay
        )
        {
            Destroy(this);
            yield break;
        }

        enemyAIBase = GetComponent<EnemyAIBase>();
        sprite = GetComponent<SpriteRenderer>().sprite;

        Vector2 intersection;
        ScreenEdge edge;
        LineIntersection2D.GetIntersectionWithScreenEdge(gameObject, transform.position, enemyAIBase.enteringTargetPosition, out intersection, out edge);

        alert = Instantiate(alertPrefab, intersection, Quaternion.identity);
        if (alert.transform.position.y > CameraUtils.CameraRect.yMax - (sprite.bounds.extents.y + offset))
            alert.transform.position += Vector3.down * (sprite.bounds.extents.y + offset);
        if (alert.transform.position.y < CameraUtils.CameraRect.yMin + (sprite.bounds.extents.y + offset))
            alert.transform.position += Vector3.up * (sprite.bounds.extents.y + offset);
        if (alert.transform.position.x < CameraUtils.CameraRect.xMin + (sprite.bounds.extents.x + offset))
            alert.transform.position += Vector3.right * (sprite.bounds.extents.x + offset);
        if (alert.transform.position.x > CameraUtils.CameraRect.xMax - (sprite.bounds.extents.x + offset))
            alert.transform.position += Vector3.left * (sprite.bounds.extents.x + offset);

    }

    public void OnBecameVisible()
    {
        StartCoroutine(HideAlertCoroutine());
    }

    private IEnumerator HideAlertCoroutine()
    {
        yield return new WaitForSeconds(hideDelay);
        if (alert)
            Destroy(alert);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Destroy(alert);
    }

    public void DestroyAlert()
    {
        if (alert)
        {
            StopAllCoroutines();
            Destroy(alert);
        }
    }
}
