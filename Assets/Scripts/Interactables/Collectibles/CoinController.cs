using UnityEngine;

public class CoinController : Collectible
{

    public bool locked;
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches coin
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            // If player isn't holding a coin
            if (!player.inventory.HasCoin())
            {
                if (locked)
                {
                    locked = !player.inventory.Unlock(); // If unlock successful, switch this to not locked
                }
                if (!locked)
                {
                    Pickup(player);
                    player.inventory.PickUpCoin(this);
                    player.AudioTrigger(PlayerAudioSignal.GEM);
                }
            }
        }
    }
}
