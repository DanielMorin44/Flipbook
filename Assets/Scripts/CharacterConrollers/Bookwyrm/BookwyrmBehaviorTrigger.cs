using UnityEngine;

public class BookwyrmBehaviorTrigger : MonoBehaviour
{
    public BookwyrmBehaviorAction action;

    public BookwyrmController agent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player" && collision is BoxCollider2D)
        {
            HandleTrigger();
            Destroy(gameObject);
        }
    }

    public void HandleTrigger()
    {
        switch (action)
        {
            case BookwyrmBehaviorAction.TopSwipe:
                agent.TopSwipe();
                break;
            case BookwyrmBehaviorAction.BottomSwipe:
                agent.BottomSwipe();
                break;
            case BookwyrmBehaviorAction.TopFireBeath:
                agent.TopFireBreath();
                break;
            case BookwyrmBehaviorAction.BottomFireBreath:
                agent.BottomFireBreath();
                break;
            case BookwyrmBehaviorAction.Chomp:
                agent.Chomp();
                break;
        }
    }

    public enum BookwyrmBehaviorAction { TopSwipe, BottomSwipe, TopFireBeath, BottomFireBreath, Chomp }
}
