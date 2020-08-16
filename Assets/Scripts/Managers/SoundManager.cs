using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource fx;
    public AudioSource music;
    public AudioSource dialogue;

    [Header("Pitch")]
    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

    public void PlaySound(AudioClip clip, AudioSource source)
    {
        if(source == fx)
        {
            float pitch = Random.Range(lowPitch, highPitch);
            source.pitch = pitch;
        }
        source.clip = clip;
        source.Play();
    }
}