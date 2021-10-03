using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuController : MonoBehaviour
{

    MainMenuController mainMenu;
    public Text selectorText;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu = GetComponentInParent<MainMenuController>();
    }

    public void SelectorTypePressed()
    {
        if (PlayerData.selectionType == PlayerData.SelectionType.Radial)
        {
            PlayerData.selectionType = PlayerData.SelectionType.Sequential;
            selectorText.text = "Selector Type: Sequential";
        } else
        {
            PlayerData.selectionType = PlayerData.SelectionType.Radial;
            selectorText.text = "Selector Type: Radial";
        }
    }

    private void OnEnable()
    {
        if (PlayerData.selectionType == PlayerData.SelectionType.Radial)
        {
            selectorText.text = "Selector Type: Radial";
        } else
        {
            selectorText.text = "Selector Type: Sequential";
        }
    }

    public void ReturnPressed()
    {
        mainMenu.SendToWelcome();
    }
}
