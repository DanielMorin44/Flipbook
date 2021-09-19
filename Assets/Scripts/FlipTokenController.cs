using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTokenController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            // If player can't flip
            if (!player.GetCanFlip())
            {
                //Destroy Token and allow player to flip
                player.SetCanFlip(true);
                Destroy(gameObject);
            }
        }
    }
}
