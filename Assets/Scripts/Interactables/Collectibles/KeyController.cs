using UnityEngine;

public class KeyController : Collectible
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            //Add a key and destroy this key
            PlayerController player = collision.GetComponent<PlayerController>();
            Pickup(player);
            player.inventory.AddKey(this);
            player.AudioTrigger(PlayerAudioSignal.GEM);
        }
    }
}
