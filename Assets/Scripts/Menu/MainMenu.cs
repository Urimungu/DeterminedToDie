using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MenuData))]
public class MainMenu : MonoBehaviour
{
    private MenuData data;
    private string menu;

    private void Start()
    {
        data = GetComponent<MenuData>();
    }

    //changes menu
    public void ChangeMenu(string menu)
    {
        //string btnPressed = EventSystem.current.currentSelectedGameObject.name;

        for (int i = 0; i < data.menus.Count; i++)
        {
            if (data.menus[i].name.Contains(menu))
            {
                Instantiate(data.menus[i], data.canvas.transform);
            }
            else
            {
                data.menus[i].SetActive(false);
            }
        }
    }

    //Quits the game and closes editor
    public void QuitBtn()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
