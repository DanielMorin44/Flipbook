using UnityEngine;

public class KeyController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            //Add a key and destroy this key
            collision.GetComponent<PlayerController>().inventory.AddKey();
            Destroy(gameObject);
        }
    }
}
