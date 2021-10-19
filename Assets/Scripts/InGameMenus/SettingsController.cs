using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public MenuController menu;
    public Text selectorText;
    public Text wallSlideText;

    public void SelectorTypePressed()
    {
        if (PlayerData.selectionType == PlayerData.SelectionType.Radial)
        {
            PlayerData.selectionType = PlayerData.SelectionType.Sequential;
            selectorText.text = "Selector Type: Sequential";
        }
        else
        {
            PlayerData.selectionType = PlayerData.SelectionType.Radial;
            selectorText.text = "Selector Type: Radial";
        }
    }

    public void HoldTypePressed()
    {
        PlayerData.wallSlideToggle = !PlayerData.wallSlideToggle;
        if (PlayerData.wallSlideToggle)
        {
            wallSlideText.text = "Wall Slide: Toggle";
        }
        else
        {
            wallSlideText.text = "Wall Slide: Hold";
        }
    }

    private void OnEnable()
    {
        if (PlayerData.selectionType == PlayerData.SelectionType.Radial)
        {
            selectorText.text = "Selector Type: Radial";
        }
        else
        {
            selectorText.text = "Selector Type: Sequential";
        }

        if (PlayerData.wallSlideToggle)
        {
            wallSlideText.text = "Wall Slide: Toggle";
        }
        else
        {
            wallSlideText.text = "Wall Slide: Hold";
        }
    }

    public void ReturnPressed()
    {
        menu.SetMainActive();
    }
}
