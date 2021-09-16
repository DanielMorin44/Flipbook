using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            //Add a key and destroy this key
            player.AddKey(1);
            Destroy(gameObject);
        }
    }
}
