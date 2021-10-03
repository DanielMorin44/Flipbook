using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public GUIController gui;

    public void ReturnPressed()
    {
        gui.SetSettingsActive(false);
    }
}
