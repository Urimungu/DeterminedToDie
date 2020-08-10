using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : Menus
{
    [Header("Sound Mixer")]
    public AudioMixer masterMixer;

    [Header("Sound Sliders")]
    public Slider slidMas;
    public Slider slidMus;
    public Slider slidDia;
    public Slider slidFX;

    private string volumeName;

    private void Start()
    {
        StartSounds();
    }

    //Switching scenes

    public void SwitchSettings(string menu)
    {
        for (int i = 0; i < _data.settingsMenus.Count; i++)
            _data.settingsMenus[i].SetActive(_data.settingsMenus[i].name.Contains(menu));
    }


    //Sound Functions

    private void StartSounds()
    {
        slidMas.value = PlayerPrefs.GetFloat("Master", 1);
        slidMus.value = PlayerPrefs.GetFloat("Music", 1);
        slidDia.value = PlayerPrefs.GetFloat("Dialogue", 1);
        slidFX.value = PlayerPrefs.GetFloat("FX", 1);
    }

    //Sets the name of vol group
    public void SetVolName(string volName)
    {
        volumeName = volName;
    }

    //Changes the sound level
    public void SetVolLevel(float volume)
    {
        masterMixer.SetFloat(volumeName, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(volumeName, volume);
    }
}