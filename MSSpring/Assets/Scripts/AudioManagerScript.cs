using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    public AudioClip[] UI;
    public AudioClip[] Environment;
    public AudioClip[] Battle;
    public AudioClip[] AnimalSounds;
    public static AudioManagerScript Instance;

    public AudioSource environmentSource;
    public AudioSource uiSource;
    public AudioSource battleSource;

    private bool isMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

/*
    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (!battleSource.isPlaying) {
                PlayUISound(UI[0]);
            }
        }
    }
    */

    public void PlayUISound(AudioClip clip)
    {
        if (!isMuted && clip != null)
            uiSource.PlayOneShot(clip);
    }

    public void PlayBattleSound(AudioClip clip)
    {
        if (!isMuted && clip != null)
            battleSource.PlayOneShot(clip);
    }

    public void PlayEnvironmentSound(AudioClip clip, bool loop = false)
    {
        if (!isMuted && clip != null)
        {
            environmentSource.clip = clip;
            environmentSource.loop = loop;
            environmentSource.Play();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        environmentSource.mute = isMuted;
        uiSource.mute = isMuted;
        battleSource.mute = isMuted;
    }

    public void FadeOutEnvironment(float duration)
    {
        StartCoroutine(FadeOut(environmentSource, duration));
    }

    private System.Collections.IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        source.Stop();
        source.volume = startVolume;
    }
}
