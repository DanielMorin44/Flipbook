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
        PlayerData.highestLevel = 0;
        SceneManager.LoadScene("Page1");
    }

    public void LoadPressed()
    {
        SaveSystem.LoadData();
        SceneManager.LoadScene("Overworld");
    }

    public void SettingsPressed()
    {
        mainMenu.SendToSettings();
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
