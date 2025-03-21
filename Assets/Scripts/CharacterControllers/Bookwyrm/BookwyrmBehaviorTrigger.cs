using UnityEngine;

public class BookwyrmBehaviorTrigger : MonoBehaviour
{
    public BookwyrmController agent;
    public BookwyrmBehaviorAction action;
    public Side triggerSide;
    public Side damageSide;
    public float delay;
    public float duration;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If player touches token
        if (collision.tag == "Player" && collision is BoxCollider2D && collision.GetComponent<SplitScreenPlayerController>().GetSide() == triggerSide)
        {
            HandleTrigger();
            Destroy(gameObject);
        }
    }

    public void HandleTrigger()
    {
        switch (action)
        {
            case BookwyrmBehaviorAction.Swipe:
                agent.Swipe(damageSide, delay);
                break;
            case BookwyrmBehaviorAction.FireBreath:
                agent.FireBreath(damageSide, delay, duration);
                break;
        }
    }

    public enum BookwyrmBehaviorAction { Swipe, FireBreath }
}
