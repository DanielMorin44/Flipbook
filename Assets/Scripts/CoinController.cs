using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{

    bool locked;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            // If player can't flip
            if (!player.HoldingCoin())
            {
                if (locked)
                {
                    locked = !player.TryUnlock(); // If unlock successful, switch this to not locked
                }
                //Destroy Coin and Give to player
                if (!locked)
                {
                    player.AddCoin();
                    Destroy(gameObject);
                }
            }
        }
    }
}
