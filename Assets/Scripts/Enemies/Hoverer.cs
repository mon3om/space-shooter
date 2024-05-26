using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverer : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float xOffset;
    public float yOffset;
    public List<Vector2> interactibleSides = new() { Vector2.left, Vector2.right };

    private Vector2 hoverSide = Vector2.right;
    private ScreenPositionLock screenPositionLock = null;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag(Tags.PLAYER_SHIP).transform;

        screenPositionLock = GetComponent<ScreenPositionLock>();
        screenPositionLock.onScreenSideReached += OnScreenSideReached;
        StartCoroutine(ChangeHoverSideCoroutine());
    }

    private void FixedUpdate()
    {
        if (!player)
        {
            player = new GameObject("hoverer scarecrow").transform;
            player.position = new(0, -2, 0);
        }

        HorizontalHover();
        VerticalHover();
    }

    private void VerticalHover()
    {
        float yTarget = transform.position.y;
        if (transform.position.y - player.position.y < yOffset)
            yTarget = transform.position.y + 1f;
        if (transform.position.y - player.position.y > yOffset + 1f)
            yTarget = transform.position.y - 1f;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, yTarget, 0), ySpeed * Time.fixedDeltaTime);
    }

    private void HorizontalHover()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x + hoverSide.x * xOffset, transform.position.y, 0), xSpeed * Time.fixedDeltaTime);
    }

    private void OnScreenSideReached(Vector2 position, Vector2 screenSide)
    {
        if (!interactibleSides.Contains(screenSide)) return;

        if (position.x > 0) hoverSide = Vector2.left;
        if (position.x < 0) hoverSide = Vector2.right;
    }

    private IEnumerator ChangeHoverSideCoroutine()
    {
        ChangeHoverSide();
        yield return new WaitForSeconds(Random.Range(3f, 10f));
        StartCoroutine(ChangeHoverSideCoroutine());
    }

    public void ChangeHoverSide()
    {
        hoverSide = new(-hoverSide.x, 0);
    }
}