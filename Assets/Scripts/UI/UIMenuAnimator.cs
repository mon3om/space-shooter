using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIMenuAnimator : MonoBehaviour
{
    [SerializeField] private Button displayButton;
    [SerializeField] private Button hideButton;
    private RectTransform rectTransform;
    private Animator logoAnimator;
    private static AudioClip swipeSound = null;

    public const int SCREEN_WIDTH = 1920;
    public const int SCREEN_HEIGHT = 1080;
    private static float speed = 0.3f;

    public static System.Action<string> OnMenuHide;

    private void OnEnable() => OnMenuHide += Hide;
    private void OnDisable() => OnMenuHide -= Hide;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        logoAnimator = GameObject.Find("GAME_LOGO").GetComponent<Animator>();

        displayButton?.onClick.AddListener(Display);
        hideButton?.onClick.AddListener(HideLogics);

        if (swipeSound == null)
            swipeSound = Resources.Load<AudioClip>("Sounds/MenuSwipe");
    }

    public void Display()
    {
        gameObject.PlaySound(swipeSound);
        float pos = rectTransform.anchoredPosition.x;
        DOTween.To(() => pos, x => rectTransform.anchoredPosition = new(x, rectTransform.anchoredPosition.y), 0, speed);
        OnMenuHide?.Invoke(name);

        if (name != "MainMenu")
            logoAnimator.SetInteger("Size", -1);
        else
            logoAnimator.SetInteger("Size", 1);
    }

    private void HideLogics()
    {
        float pos = rectTransform.anchoredPosition.x;
        DOTween.To(() => pos, x => rectTransform.anchoredPosition = new(x, rectTransform.anchoredPosition.y), -SCREEN_WIDTH, speed)
        .OnComplete(() => rectTransform.anchoredPosition = new(SCREEN_WIDTH, rectTransform.anchoredPosition.y));
    }

    public void Hide(string uIMenuAnimatorName)
    {
        if (uIMenuAnimatorName == name) return;

        HideLogics();
    }

    public void Hide() => HideLogics();
}
