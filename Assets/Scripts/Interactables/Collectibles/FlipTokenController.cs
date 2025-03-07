using UnityEngine;

public class FlipTokenController : Collectible
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            // If player can't flip
            if (!player.inventory.GetFlipToken())
            {
                Pickup(player);
                player.inventory.AddFlipToken(this);
                player.AudioTrigger(PlayerAudioSignal.GEM);
            }
        }
    }
}
