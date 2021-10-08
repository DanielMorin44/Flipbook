using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    //public GUIController gui;
    public SettingsController settings;
    public MainController main;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainActive()
    {
        main.gameObject.SetActive(true);
        settings.gameObject.SetActive(false);
    }

    public void SetSettingsActive()
    {
        settings.gameObject.SetActive(true);
        main.gameObject.SetActive(false);
    }
}
