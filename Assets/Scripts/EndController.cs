using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    public PlayerController player;
    public string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            // End the stage
            Debug.Log("Stage Complete");
            SceneManager.LoadScene(nextScene);
        }
    }
}
