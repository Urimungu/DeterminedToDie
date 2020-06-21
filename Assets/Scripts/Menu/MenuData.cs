using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MenuData : MonoBehaviour
{
    [Header("List of Main Menus")]
    public List<GameObject> menus = new List<GameObject>();

    [Header("List of Setting Menus")]
    public List<GameObject> settingsMenus = new List<GameObject>();
}