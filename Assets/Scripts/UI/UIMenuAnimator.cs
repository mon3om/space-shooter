using UnityEngine;

enum AnimatorState { Entering, Leaving, Disabled, Enabled }
public class UIMenuAnimator : MonoBehaviour
{
    [SerializeField] private float animationSpeed;
    private AnimatorState animatorState = AnimatorState.Disabled;
    private RectTransform rectTransform;

    public const int SCREEN_WIDTH = 1920;
    public const int SCREEN_HEIGHT = 1080;
    private static float speed = 0;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (speed == 0)
            speed = animationSpeed;
    }

    private void Update()
    {
        if (animatorState == AnimatorState.Disabled || animatorState == AnimatorState.Enabled) return;

        float pos = rectTransform.anchoredPosition.x;

        if (animatorState == AnimatorState.Entering)
        {
            if (rectTransform.anchoredPosition.x > 0)
            {
                pos = Mathf.Lerp(pos, -100, speed);
            }
            else
            {
                pos = 0;
                animatorState = AnimatorState.Enabled;
            }
        }

        if (animatorState == AnimatorState.Leaving)
        {
            if (rectTransform.anchoredPosition.x > -SCREEN_WIDTH)
            {
                pos = Mathf.Lerp(pos, -SCREEN_WIDTH - 100, speed);
            }
            else
            {
                pos = -SCREEN_WIDTH;
                animatorState = AnimatorState.Disabled;
            }
        }

        rectTransform.anchoredPosition = new(pos, rectTransform.anchoredPosition.y);
    }

    public void Display()
    {
        rectTransform.anchoredPosition = new(SCREEN_WIDTH, rectTransform.anchoredPosition.y);
        animatorState = AnimatorState.Entering;
    }

    public void Hide()
    {
        rectTransform.anchoredPosition = new(0, rectTransform.anchoredPosition.y);
        animatorState = AnimatorState.Leaving;
    }
}
