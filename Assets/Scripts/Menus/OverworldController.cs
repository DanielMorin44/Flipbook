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
    public void Level1_1Pressed()
    {
        SceneManager.LoadScene("Page1");
    }

    public void Level1_2Pressed()
    {
        SceneManager.LoadScene("Page2");
    }

    public void Level1_3Pressed()
    {
        SceneManager.LoadScene("Page3");
    }

    public void Level1_4Pressed()
    {
        SceneManager.LoadScene("Page4");
    }

    public void Level1_5Pressed()
    {
        SceneManager.LoadScene("Page5");
    }

    public void Level1_6Pressed()
    {
        SceneManager.LoadScene("Page6");
    }

    public void Level1_7Pressed()
    {
        SceneManager.LoadScene("Page7");
    }
    public void Level1_8Pressed()
    {
        SceneManager.LoadScene("Page8");
    }

    public void Level1_9Pressed()
    {
        SceneManager.LoadScene("Page9");
    }

    public void Level1_10Pressed()
    {
        SceneManager.LoadScene("Page10");
    }

    public void ReturnPressed()
    {
        SceneManager.LoadScene("WelcomeScreen");
    }
    #endregion
}
