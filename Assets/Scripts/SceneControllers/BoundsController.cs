using UnityEngine;


public class BoundsController : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If player leaves the bounds
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().Kill();
        }
    }
}
