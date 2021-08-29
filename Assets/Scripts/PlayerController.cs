using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public vars
    public float speed;
    public float jumpForce;

    // private state vars
    bool canJump, shouldJump;
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
        position.x = transform.position.x + (horizontalMove * speed * Time.deltaTime);
        transform.position = position;

        if (shouldJump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If we collide with terrain
        if(collision.gameObject.tag == "terrain")
        {
            // If the bottom edge collided with terrain
            if (collision.otherCollider.GetType() == typeof(EdgeCollider2D))
            {
                canJump = true;
                Debug.Log("CanJump == true");
            }
        }
    }

    /*private void OnCollisionExit2D(Collision2D collision)
    {
        // If we leave collision with terrain
        if (collision.gameObject.tag == "terrain")
        {
            // If the bottom edge left the collision with terrain
            if (collision.otherCollider.GetType() == typeof(EdgeCollider2D))
            {
                canJump = false;
                Debug.Log("CanJump == false");
            }
        }
    }*/
}
