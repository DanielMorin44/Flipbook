using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject[] flipCandidates;

    public int curLevelIndex = 0;

    public bool FlipLevel(int index)
    {
        if (index >= flipCandidates.Length)
        {
            Debug.Log("Out of Bounds Index Selected");
            return false;
        }
        if(index == curLevelIndex)
        {
            Debug.Log("Selected Level same as current");
            return false;
        }
        Debug.Log("Level Manager Flipping");
        flipCandidates[index].SetActive(true);
        flipCandidates[curLevelIndex].SetActive(false);
        curLevelIndex = index;
        return true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void PlayerDied()
    {
        Debug.Log("You Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}