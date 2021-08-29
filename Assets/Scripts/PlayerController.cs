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

    Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("A");
        // If we collide with terrain
        if (collision.gameObject.tag == "terrain")
        {
            Debug.Log(collision.otherCollider.GetType());
            // If the bottom edge collided with terrain
            if (collision.otherCollider.GetType() == typeof(EdgeCollider2D))
            {
                canJump = true;
                Debug.Log("CanJump == true");
                inAir = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If we leave collision with terrain
        if (collision.gameObject.tag == "terrain")
        {
            // If the bottom edge left the collision with terrain
            if (collision.otherCollider.GetType() == typeof(EdgeCollider2D))
            {
                inAir = true;
            }
        }
    }
}
