using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    public string nextScene;
    public bool locked;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (locked)
            {
                locked = !player.TryUnlock(); // If unlock successful, switch this to not locked
            }
            // End the stage
            if (!locked)
            {
                Debug.Log("Stage Complete");
                SceneManager.LoadScene(nextScene);
            }
        }
    }
}
