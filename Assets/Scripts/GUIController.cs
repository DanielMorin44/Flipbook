using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public HUDController hud;
    public MenuController menu;
    public SettingsController settings;
    public GameObject radialSelector;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMenuActive(bool active)
    {
        menu.gameObject.SetActive(active);
        hud.gameObject.SetActive(!active);
    }

    public void SetSettingsActive(bool active)
    {
        menu.gameObject.SetActive(!active);
        settings.gameObject.SetActive(active);
    }

    public void OpenSelector()
    {
        radialSelector.SetActive(true);
    }

    public void CloseSelector()
    {
        radialSelector.SetActive(false);
    }
}
