using UnityEngine;

public class Lever : MonoBehaviour
{
    protected bool pulled;

    public Animator anim;

    //objects to be switched
    public SwitchBlockController[] affectedSwitchBlocks;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        // If player touches lever
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            SwitchLever();
            player.AudioTrigger(PlayerAudioSignal.LEVER);
        }
    }

    private void SwitchLever()
    {
        pulled = !pulled;

        foreach (SwitchBlockController sbc in affectedSwitchBlocks){
            sbc.Flip();
        }

        anim.SetBool("pulled", pulled);
    }
}
