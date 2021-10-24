using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject[] flipCandidates;
    public GameObject[] ends;
    public Collider2D playerCollider;

    public int activeLevel;
    private int curPageIndex;


    // Start is called before the first frame update
    void Start()
    {
        activeLevel = PlayerData.pageToLoad;
        //Set Player's Location.
        foreach (Transform t in flipCandidates[activeLevel].transform)
        {
            if (t.name == "PlayerStart")
            {
                player.transform.position = t.position;
            }
        }
        //Activate Page
        SetOnlyOpen(activeLevel);
        //Activate End
        ends[activeLevel].SetActive(true);
        curPageIndex = activeLevel;
    }

    public bool FlipLevel(int index)
    {
        if (index >= flipCandidates.Length)
        {
            Debug.Log("Out of Bounds Index Selected");
            return false;
        }
        if(index == curPageIndex)
        {
            Debug.Log("Selected Level same as current");
            return false;
        }
        if (!CheckFlipAllowed(index))
        {
            Debug.Log("Terrain in the way!");
            return false;
        }
        flipCandidates[curPageIndex].SetActive(false);
        curPageIndex = index;
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

    private bool CheckFlipAllowed(int index)
    {
        flipCandidates[index].SetActive(true);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(playerCollider.bounds.center, 
                                                        new Vector2(playerCollider.bounds.size.x, playerCollider.bounds.size.y * .9f),
                                                        0,
                                                        LayerMask.GetMask("terrain"));

        for (int i = 0; i < colliders.Length; i++)
        {
            Component collider = flipCandidates[index].GetComponentInChildren(typeof(Grid)).GetComponentInChildren(typeof(CompositeCollider2D));
            if (colliders[i].gameObject.name == collider.gameObject.name)
            {
                flipCandidates[index].SetActive(false);
                return false;
            }
        }
        return true;
    }

    public int GetCurrentPage()
    {
        return curPageIndex;
    }

    public void Open(int index)
    {
        if (index > flipCandidates.Length-1)
        {
            Debug.Log("Page out of scope");
            return;
        }
        flipCandidates[index].SetActive(true);
    }

    public void Close(int index)
    {
        if (index > flipCandidates.Length-1)
        {
            Debug.Log("Page out of scope");
            return;
        }
        flipCandidates[index].SetActive(false);
    }

    public void SetOnlyOpen(int index)
    {
        if (index > flipCandidates.Length-1)
        {
            Debug.Log("Page out of scope");
            return;
        }
        for (int i = 0; i < flipCandidates.Length; i++)
        {
            Close(i);
        }
        Open(index);
    }

    public int GetNumberPagesAvailable()
    {
        return activeLevel + 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(playerCollider.bounds.center, new Vector2(playerCollider.bounds.size.x, playerCollider.bounds.size.y * .9f));
    }

    public void CompleteLevel(int page, string nextLevel)
    {
        if (activeLevel > PlayerData.highestLevel)
        {
            PlayerData.highestLevel = activeLevel;
        }
        PlayerData.pageToLoad = page;
        SceneManager.LoadScene(nextLevel);
    }

}