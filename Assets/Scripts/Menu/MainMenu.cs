using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MenuData))]
public class MainMenu : MonoBehaviour
{
    protected MenuData _data;

    private void Start()
    {
        _data = GetComponent<MenuData>();
    }

    //changes menu
    public void ChangeMenu(string menu)
    {
        for (int i = 0; i < _data.menus.Count; i++)
            _data.menus[i].SetActive(_data.menus[i].name.Contains(menu));
    }

    //Quits the game and closes editor
    public void QuitBtn()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}