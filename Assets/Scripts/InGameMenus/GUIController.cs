using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public HUDController hud;
    public MenuController menu;
    public GameObject radialSelector;
    public GameObject dialog;

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
        menu.SetMainActive();
    }

    public void DialogueOpen()
    {
        dialog.SetActive(true);
    }

    public void DialogueClose()
    {
        dialog.SetActive(false);
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
