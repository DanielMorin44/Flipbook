using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public vars
    public float groundHorizontalSpeed;
    public float inAirHorizontalSpeed;
    public float jumpForce;

    // private state vars
    bool canJump, shouldJump;
    bool inAir;
    float horizontalMove;
    bool canFlip;

    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
        canFlip = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        // We should jump if user is pressing jump button and the player is allowed to jump
        shouldJump = (Input.GetAxis("Jump") != 0) && canJump;
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        float speed;
        if (shouldJump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }

        if (!inAir)
        {
            speed = groundHorizontalSpeed;
        }
        else
        {
            speed = inAirHorizontalSpeed;
        }
        position.x = transform.position.x + (horizontalMove * speed * Time.deltaTime);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the bottom edge collided with terrain
        if (collision.gameObject.tag == "terrain")
        {
            canJump = true;
            inAir = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we leave collision with terrain
        if (collision.gameObject.tag == "terrain")
        {
            inAir = true;
        }
    }

    public bool getFlipValue()
    {
        return canFlip;
    }
    
    public void setFlipValue(bool value)
    {
        canFlip = value;
    }

}
