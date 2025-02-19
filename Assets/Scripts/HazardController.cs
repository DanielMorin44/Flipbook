using UnityEngine;

public class HazardController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().Kill();
        }
    }
}
