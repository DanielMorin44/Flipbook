using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookThroughController : MonoBehaviour
{
    public LevelManager levelManager;
    public GameObject[] buttons;

    public void OnEnable()
    {
        for (int i = levelManager.GetNumberPagesAvailable(); i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    public void buttonSelected(int page)
    {
        if (page < levelManager.GetNumberPagesAvailable())
        {
            levelManager.SetOnlyOpen(page);
        }
    }
}
