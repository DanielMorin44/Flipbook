using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    public string tipText;
    private HUDController hudController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hudController = FindFirstObjectByType<HUDController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches
        if (collision.tag == "Player")
        {
            hudController.ShowTip(tipText);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player leaves
        if (collision.tag == "Player")
        {
            hudController.HideTip();
        }
    }
}
