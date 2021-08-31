using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject[] flipCandidates;

    public bool level0;

    public void FlipLevel()
    {
        Debug.Log("Level Manager Flipping");
        if (level0)
        {
            flipCandidates[0].SetActive(true);
            flipCandidates[1].SetActive(false);
        }
        else
        {
            flipCandidates[0].SetActive(false);
            flipCandidates[1].SetActive(true);
        }
        level0 = !level0;
    }

    public void PlayerDied()
    {
        Debug.Log("You Died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}