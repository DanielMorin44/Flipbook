using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Collectible : MonoBehaviour
{

    private float targetPositionOffset = 1f;
    private Vector3 velocity = Vector3.zero;
    private PlayerController owner = null;
    private float smoothTime = .2f;
    private Transform followTarget;

    void Update()
    {
        if(owner != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, followTarget.position - new Vector3(targetPositionOffset * owner.facing, 0, 0), ref velocity, smoothTime);
        }
    }

    public void Pickup(PlayerController player)
    {
        owner = player;
        transform.parent = null;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void SetFollowTarget(Transform follow)
    {
        followTarget = follow;
    }
}
