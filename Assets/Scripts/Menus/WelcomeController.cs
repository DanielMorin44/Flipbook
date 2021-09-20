using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeController : MonoBehaviour
{
    MainMenuController mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GetComponentInParent<MainMenuController>();
    }

    public void NewGamePressed()
    {
        SceneManager.LoadScene("Page0");
    }

    public void LoadPressed()
    {
        Debug.Log("Load Not Implemented");
    }

    public void SettingsPressed()
    {
        //Debug.Log("Settings Not Implemented");
        mainMenu.SendToSettings();
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
