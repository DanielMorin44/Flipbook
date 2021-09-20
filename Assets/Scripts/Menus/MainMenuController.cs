using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject welcome;
    public GameObject settings;
    private void Start()
    {
    }

    public void SendToWelcome()
    {
        welcome.SetActive(true);
        settings.SetActive(false);
    }

    public void SendToSettings()
    {
        welcome.SetActive(false);
        settings.SetActive(true);
    }
}
