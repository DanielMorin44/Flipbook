using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoundsController : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            //Reset Scene
            player.Kill();
        }
    }
}
