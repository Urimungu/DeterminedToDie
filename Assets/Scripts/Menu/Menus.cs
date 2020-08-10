using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MenuData))]
public class Menus : MonoBehaviour
{
    protected MenuData _data;

    private void Start()
    {
        _data = GetComponent<MenuData>();
        ChangeMenu("StartScreen");
    }

    //Changes Menu
    public void ChangeMenu(string menu)
    {
        for (int i = 0; i < _data.menus.Count; i++)
            _data.menus[i].SetActive(_data.menus[i].name.Contains(menu));
    }

    //Changes Scene
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    //Quits the game and closes editor
    public void QuitBtn()
    {
        //EditorApplication.isPlaying = false;
        Application.Quit();
    }
}