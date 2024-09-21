using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnhancedHoverer : MonoBehaviour
{
    private Vector2 hoverSide = Vector2.right;
    private ScreenPositionLock screenPositionLock = null;

    private Transform player;
    private bool isHovering = false;
    private List<Vector2> availablePositions = new List<Vector2>();

    public float tweenSpeed = 1;

    private void Start()
    {
        player = GameObject.FindWithTag(Tags.PLAYER_SHIP).transform;

        screenPositionLock = GetComponent<ScreenPositionLock>();
        screenPositionLock.onScreenSideReached += OnScreenSideReached;

        availablePositions = Utils.DivideScreen(5, 4, 2.5f);
    }

    private void FixedUpdate()
    {
        if (!isHovering) return;

        if (!player)
        {
            player = new GameObject("HovererScarecrow").transform;
            player.position = new(0, -2, 0);
        }
    }

    private void Hover()
    {
        transform.DOMove(new(hoverSide.x, hoverSide.y, 0), Vector2.Distance(transform.position, hoverSide) / tweenSpeed).SetEase(Ease.InOutCirc);
    }

    private void OnScreenSideReached(Vector2 position, Vector2 screenSide)
    {

    }

    private IEnumerator ChangeHoverSideCoroutine()
    {
        var nextHover = ChangeHoverSide();
        yield return new WaitForSeconds(2f);
        hoverSide = nextHover;
        Hover();
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        StartCoroutine(ChangeHoverSideCoroutine());
    }

    public Vector3 ChangeHoverSide()
    {
        return availablePositions[Random.Range(0, availablePositions.Count)];
    }

    public void SetHovering(bool isHovering)
    {
        this.isHovering = isHovering;
        StopAllCoroutines();

        if (isHovering)
            StartCoroutine(ChangeHoverSideCoroutine());

    }
}