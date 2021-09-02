using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public vars
    public float groundHorizontalSpeed;
    public float inAirHorizontalSpeed;
    public float jumpForce;
    public LevelManager levelManager;
    public InputController inputController;

    // private state vars
    bool canJump, shouldJump;
    bool canFlip, shouldFlip;
    bool shouldReset;
    bool inAir;
    float horizontalMove;

    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
        canFlip = false;
        shouldReset = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        float speed;
        if (shouldReset)
        {
            ResetPlayer();
            return;
        }
        if (shouldJump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
        if (shouldFlip)
        {
            Flip();
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

    private void Flip()
    {
        shouldFlip = false;
        // CanFlip = true if flip failed
        inputController.InitiateFlip();
    }

    public void FlipSuccess()
    {
        canFlip = false;
    }

    public bool GetFlipValue()
    {
        return canFlip;
    }

    public void SetFlipValue(bool value)
    {
        canFlip = value;
    }

    public void SetShouldReset(bool value)
    {
        shouldReset = value;
    }

    public void SetHorizontalMove(float value)
    {
        horizontalMove = value;
    }

    public void SetShouldJump(bool value)
    {
        shouldJump = value;
    }

    public bool GetCanJump()
    {
        return canJump;
    }

    public void SetShouldFlip(bool value)
    {
        shouldFlip = value;
    }

    public bool GetCanFlip()
    {
        return canFlip;
    }

    public void Kill()
    {
        levelManager.PlayerDied();
    }

    private void ResetPlayer()
    {
        SetShouldReset(false);
        Kill();
    }

}
