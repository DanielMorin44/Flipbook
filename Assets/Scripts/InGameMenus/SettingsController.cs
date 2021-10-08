using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public MenuController menu;

    public void ReturnPressed()
    {
        menu.SetMainActive();
    }
}
