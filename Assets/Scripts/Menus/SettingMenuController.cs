using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenuController : MonoBehaviour
{

    MainMenuController mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GetComponentInParent<MainMenuController>();
    }

    public void ReturnPressed()
    {
        mainMenu.SendToWelcome();
    }
}
