using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Sources")]
    public AudioSource fxSource;
    public AudioSource musicSource;
    public AudioSource dialogueSource;

    private AudioSource source;

    [Header("Audio Mixers")]
    public AudioMixerGroup fxGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup dialogueGroup;

    private AudioMixerGroup group;

    [Header("Pitch")]
    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

    public void PlaySound(AudioClip clip, string sourceName, bool changePitch, bool loop)
    {
        SourceOutput(sourceName);

        if (source == null)
            return;

        if(changePitch)
        {
            float pitch = Random.Range(lowPitch, highPitch);
            source.pitch = pitch;
        }

        source.outputAudioMixerGroup = group;
        source.loop = loop;
        source.clip = clip;
        source.Play();
    }

    void SourceOutput(string name)
    {
        switch(name.ToLower())
        {
            case "music":
                source = musicSource;
                group = musicGroup;
                break;
            case "fx":
                source = fxSource;
                group = fxGroup;
                break;
            case "dialogue":
                source = dialogueSource;
                group = dialogueGroup;
                break;
            default:
                Debug.LogError("The options for source are music, fx, and dialogue: SoundManager");
                break;

        }
    }
}