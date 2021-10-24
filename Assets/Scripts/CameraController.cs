using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D sceneBounds;

    public float cameraPanSpeed;

    private bool panMode = false;
    private float horizontalMove;
    private float verticalMove;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!panMode)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        } else
        {
            float newX = transform.position.x + (horizontalMove * cameraPanSpeed * Time.unscaledDeltaTime);
            float newY = transform.position.y + (verticalMove * cameraPanSpeed * Time.unscaledDeltaTime);
            if (sceneBounds.bounds.Contains(new Vector3(newX, newY, sceneBounds.gameObject.transform.position.z)))
            {
                transform.position = new Vector3(newX, newY, transform.position.z);
            }
        }
    }

    public void SetHorizontalMove(float move)
    {
        horizontalMove = move;
    }

    public void SetVerticalMove(float move)
    {
        verticalMove = move;
    }

    public void SetPanMode(bool mode)
    {
        panMode = mode;
    }
}
