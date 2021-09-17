using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

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
            transform.position = new Vector3(transform.position.x + (horizontalMove * cameraPanSpeed * Time.unscaledDeltaTime), 
                                             transform.position.y + (verticalMove * cameraPanSpeed * Time.unscaledDeltaTime), 
                                             transform.position.z);
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
