using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenu : MonoBehaviour
{
    //Loads the Settings
    public void SettingsBtn()
    {
        SceneManager.LoadScene(1);
    }

    //Loads the characterSelection
    public void PlayBtn()
    {
        SceneManager.LoadScene(2);
    }

    //Quits the game and closes editor
    public void QuitBtn()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
