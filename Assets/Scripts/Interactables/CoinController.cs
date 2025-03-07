using UnityEngine;

public class CoinController : MonoBehaviour
{

    public bool locked;
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches coin
        if (collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            // If player isn't holding a coin
            if (!player.inventory.HasCoin())
            {
                if (locked)
                {
                    locked = !player.inventory.Unlock(); // If unlock successful, switch this to not locked
                }
                //Destroy Coin and Give to player
                if (!locked)
                {
                    player.inventory.SetCoinId(id);
                    player.AudioTrigger(PlayerAudioSignal.GEM);
                    Destroy(gameObject);
                }
            }
        }
    }
}
