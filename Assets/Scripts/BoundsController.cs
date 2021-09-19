using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoundsController : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            //Reset Scene
            player.Kill();
        }
    }
}
