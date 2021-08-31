using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoundsController : MonoBehaviour
{
    public PlayerController player;
    private bool justFlipped = true;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            Debug.Log("Player Left bounds");
            if (justFlipped)
            {
                justFlipped = false;
            }
            else
            {
                //Reset Scene
                player.Kill();
            }
        }
    }
}
