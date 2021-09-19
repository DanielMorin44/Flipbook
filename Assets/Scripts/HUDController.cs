using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Text flipText;
    public Text numKeysText;
    public PlayerController player;

    private bool menuActive;

    // Start is called before the first frame update
    void Start()
    {
        menuActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        numKeysText.text = "Num Keys: " + player.GetNumKeys().ToString();
        flipText.text = player.GetCanFlip() ? "Can Flip = true" : "Can Flip = false";
    }

    public void SetMenuActive(bool state)
    {
        menuActive = state;
    }
}
