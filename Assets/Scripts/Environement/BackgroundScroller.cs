using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float ySpeed;
    public float xSpeed;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.size += new Vector2(xSpeed, ySpeed) * Time.deltaTime;
    }
}
