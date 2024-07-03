using System.Collections;
using UnityEngine;

public class EnhancedHoverer : Hoverer
{
    private Vector2 hoverSide = Vector2.right;
    private ScreenPositionLock screenPositionLock = null;

    private Transform player;
    private bool isHovering = false;

    private void Start()
    {
        player = GameObject.FindWithTag(Tags.PLAYER_SHIP).transform;

        screenPositionLock = GetComponent<ScreenPositionLock>();
        screenPositionLock.onScreenSideReached += OnScreenSideReached;
    }

    private void FixedUpdate()
    {
        if (!isHovering) return;

        if (!player)
        {
            player = new GameObject("hoverer scarecrow").transform;
            player.position = new(0, -2, 0);
        }

        Hover();
    }

    private void Hover()
    {
        transform.position = Vector3.Slerp(transform.position, new Vector3(hoverSide.x, hoverSide.y, 0), ySpeed * Time.fixedDeltaTime);
    }

    private void OnScreenSideReached(Vector2 position, Vector2 screenSide)
    {

    }

    [Space]
    public GameObject nextHoverAlert;
    private IEnumerator ChangeHoverSideCoroutine()
    {
        var nextHover = ChangeHoverSide();
        var alert = Instantiate(nextHoverAlert, nextHover, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Destroy(alert);
        hoverSide = nextHover;
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        StartCoroutine(ChangeHoverSideCoroutine());
    }

    public new Vector3 ChangeHoverSide()
    {
        return new(Random.Range(CameraUtils.CameraRect.xMin, CameraUtils.CameraRect.xMax), Random.Range(CameraUtils.CameraRect.yMin, CameraUtils.CameraRect.yMax));
    }

    public void SetHovering(bool isHovering)
    {
        this.isHovering = isHovering;
        if (isHovering)
        {
            StartCoroutine(ChangeHoverSideCoroutine());
        }
        else
        {
            StopCoroutine(ChangeHoverSideCoroutine());
        }
    }
}