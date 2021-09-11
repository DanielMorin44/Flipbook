using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject[] flipCandidates;
    public Collider2D playerCollider;

    public int curLevelIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
    }

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
        if (!CheckFlipAllowed(index))
        {
            Debug.Log("Terrain in the way!");
            return false;
        }

        Debug.Log("Level Manager Flipping");
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
}