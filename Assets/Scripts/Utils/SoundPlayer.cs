using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public List<AudioClip> clips;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string name)
    {
        var audioClip = GetAudioClipByName(name);
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayStandalone(string name)
    {
        var audioClip = GetAudioClipByName(name);
        var newGO = new GameObject("Temp SoundPlayer");
        var source = newGO.AddComponent<AudioSource>();
        source.clip = audioClip;
        source.Play();
        Destroy(newGO, audioClip.length);
    }

    public void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayStandalone(AudioClip clip)
    {
        var newGO = new GameObject("Temp SoundPlayer");
        var source = newGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(newGO, clip.length);
    }

    private AudioClip GetAudioClipByName(string name)
    {
        var clip = clips.Find(x => x.name == name);
        if (clip == null)
            throw new System.Exception($"Clip [{name}] not found - GetAudioClipByName()");
        return clip;
    }
}