using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    [Header("Appearance Settings")]
    public float displayDelay = 1;
    public float textDelay = 2f;

    [Space]
    public StoryScriptableObject storyChunk;
    [SerializeField] private Image imageNPC;
    [SerializeField] private Image imagePlayer;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private float initTextSpeed;
    [SerializeField] private float speedMultiplier;

    private int chunckCounter = 0;
    private Tween tween = null;
    private float tweenSpeed;

    private System.Action onComplete;
    private bool chunkDisplayed = false;
    private bool processCompleted = false;

    private void Start()
    {
        Instances.StoryManager = this;
        tweenSpeed = textDelay;
        if (Instances.AuthInfo.profileTexture != null)
            imagePlayer.sprite = Sprite.Create(Instances.AuthInfo.profileTexture, new Rect(0.0f, 0.0f, Instances.AuthInfo.profileTexture.width, Instances.AuthInfo.profileTexture.height), new Vector2(0, 0), 100.0f);
    }

    private void Update()
    {
        if (processCompleted && !storyText.gameObject.activeSelf) return;

        if (Input.anyKeyDown)
        {
            if (processCompleted)
            {
                Hide();
                onComplete?.Invoke();
                onComplete = null;
                return;
            }

            if (!chunkDisplayed)
                SpeedUpTheTween();
            else
                DisplayText();
        }
    }

    public void StartStoryTelling(System.Action onComplete, StoryScriptableObject story)
    {
        this.onComplete = onComplete;
        processCompleted = false;
        storyChunk = story;
        tweenSpeed = initTextSpeed;
        chunckCounter = 0;
        storyText.text = "";
        Display();
    }

    public void Display()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GameObject background = imageNPC.transform.parent.Find("Background").gameObject;
        StartCoroutine(CoroutinedAction(() => background.SetActive(true), 0));
        StartCoroutine(CoroutinedAction(() => imageNPC.gameObject.SetActive(true), displayDelay));
        StartCoroutine(CoroutinedAction(() => imagePlayer.gameObject.SetActive(true), displayDelay));
        StartCoroutine(CoroutinedAction(() => storyText.transform.parent.gameObject.SetActive(true), displayDelay));

        StartCoroutine(CoroutinedAction(() => DisplayText(), textDelay));
    }

    private void DisplayText()
    {
        storyText.text = "";
        tween = storyText.DOText(storyChunk.chuncks[chunckCounter], storyChunk.chuncks[chunckCounter].Length / 20f);
        tween.onComplete = () => OnTextDisplayComplete();
        tween.timeScale = 1;
        chunkDisplayed = false;
    }

    private void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator CoroutinedAction(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    private void SpeedUpTheTween()
    {
        if (tweenSpeed == initTextSpeed) tweenSpeed *= speedMultiplier;
        else if (tweenSpeed > initTextSpeed) tweenSpeed = 500;
        if (tween != null)
            tween.timeScale = tweenSpeed;
    }

    private void OnTextDisplayComplete()
    {
        if (chunckCounter < storyChunk.chuncks.Count - 1)
        {
            chunckCounter++;
            chunkDisplayed = true;
            tweenSpeed = initTextSpeed;
        }
        else
        {
            processCompleted = true;
            return;
        }
    }
}
