using UnityEngine;

public class EndController : MonoBehaviour
{
    public bool locked;
    public InputController inputController;
    public LevelManager levelManager;
    public string nextLevel;
    public int nextPage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player")
        {
            if (!inputController.IsPlayerControl())
            {
                return;
            }
            PlayerController player = collision.GetComponent<PlayerController>();
            if (locked)
            {
                locked = !player.TryUnlock(); // If unlock successful, switch this to not locked
            }
            // End the stage
            if (!locked)
            {
                levelManager.CompleteLevel(nextPage, nextLevel);
            }
        }
    }
}
