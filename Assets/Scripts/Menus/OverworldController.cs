using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverworldController : MonoBehaviour
{
    public Button[] levelSelectButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
   void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = PlayerData.highestLevel+1; i < levelSelectButtons.Length; i++)
        {
            levelSelectButtons[i].interactable = false;
        }
    }

    #region Button Methods
    public void Chapter1ButtonPressed(int page)
    {
        PlayerData.pageToLoad = page;
        SceneManager.LoadScene("Chapter1");
    }

    public void ReturnPressed()
    {
        SceneManager.LoadScene("WelcomeScreen");
    }
   
    public void QuitPressed()
    {
        Application.Quit();
    }
    #endregion
}
