using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MenuData : MonoBehaviour
{
    [Header("Menus: AutoFill")]
    public List<GameObject> menus = new List<GameObject>();
    public GameObject canvas;

    private GameObject[] gameMenus;

    private void Start()
    {
        menus.Clear();
        gameMenus = Resources.LoadAll("Prefabs/Menus", typeof(GameObject)).Cast<GameObject>().ToArray();

        for (int i = 0; i < gameMenus.Length; i++)
        {
            menus.Add(gameMenus[i]);
        }
    }
}