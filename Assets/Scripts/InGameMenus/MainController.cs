using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public MenuController menu;

    private InputController input;
    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.FindObjectOfType<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumePressed()
    {
        input.PauseFinished();
    }

    public void ReturnToMenuPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Overworld");
    }

    public void SettingsPressed()
    {
        menu.SetSettingsActive();
    }

    public void SavePressed()
    {
        SaveSystem.SaveData();
        Debug.Log("Game Saved!");
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
