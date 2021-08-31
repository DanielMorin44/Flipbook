using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            // End the stage
        }
    }
}
