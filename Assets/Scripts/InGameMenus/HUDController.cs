using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public Text numKeysText;
    public TextMeshProUGUI pageNumberText;
    public PlayerController player;
    public LevelManager levelManager;

    // Update is called once per frame
    void Update()
    {
        pageNumberText.text = "Page: " + (levelManager.GetOpenedPage()+1).ToString();
    }
}
