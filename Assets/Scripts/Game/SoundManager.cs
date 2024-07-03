using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip initClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initClip = audioSource.clip;
    }

    public void PlaySound()
    {
        if (initClip != null)
        {
            audioSource.clip = initClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip is null");
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("Audio clip is null");
            return;
        }
        audioSource.clip = clip;
        audioSource.Play();
    }
}
