using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Text numKeysText;
    public Text pageNumberText;
    public PlayerController player;
    public LevelManager levelManager;

    // Update is called once per frame
    void Update()
    {
        pageNumberText.text = "Page: " + (levelManager.GetOpenedPage()+1).ToString();
    }
}
