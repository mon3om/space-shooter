using UnityEngine;

public static class SoundExtensions
{
    public static void PlaySound(this GameObject gameobject, AudioClip clip)
    {
        if (gameobject == null)
        {
            Debug.LogError("Null Gameobject");
            return;
        }

        AudioSource audioSource = gameobject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = false;
        audioSource.Play();
    }

}