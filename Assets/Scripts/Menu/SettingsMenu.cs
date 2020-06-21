using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MainMenu
{
    public void SwitchSettings(string menu)
    {
        for (int i = 0; i < _data.settingsMenus.Count; i++)
            _data.settingsMenus[i].SetActive(_data.settingsMenus[i].name.Contains(menu));
    }
}