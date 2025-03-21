using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI pageNumberText;
    public TextMeshProUGUI tipText;
    public PlayerController player;
    public LevelManager levelManager;

    // Update is called once per frame
    void Update()
    {
        pageNumberText.text = "Page: " + (levelManager.GetOpenedPage()+1).ToString();
    }

    public void ShowTip(string tip)
    {
        tipText.SetText(tip);
        tipText.enabled = true;
    }

    public void HideTip()
    {
        tipText.SetText("");
        tipText.enabled = false;
    }
}
