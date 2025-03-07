using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Text flipText;
    public Text numKeysText;
    public Text pageNumberText;
    public PlayerController player;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        numKeysText.text = "Num Keys: " + player.inventory.GetKeys().ToString();
        flipText.text = player.inventory.GetFlipToken() ? "Can Flip = true" : "Can Flip = false";
        pageNumberText.text = "Page: " + (levelManager.GetOpenedPage()+1).ToString();
    }
}
