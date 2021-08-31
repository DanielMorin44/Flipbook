using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTokenController : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            // If player can't flip
            if (!player.GetFlipValue())
            {
                //Destroy Token and allow player to flip
                player.SetFlipValue(true);
                Destroy(gameObject);
            }
        }
    }
}
