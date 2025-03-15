using UnityEngine;

public class SoundShifter : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip targetClip;
    public float shiftDuration = 0.3f;

    private bool isReducing = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isReducing && audioSource.volume >= 1) return;

        audioSource.volume += 1 / shiftDuration * Time.deltaTime * (isReducing ? -1 : 1);
        // Debug.Log(audioSource.volume);
        if (audioSource.volume <= 0)
        {
            isReducing = false;
            audioSource.clip = targetClip;
            audioSource.Play();
        }
    }
}
