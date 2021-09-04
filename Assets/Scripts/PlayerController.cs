using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public vars
    public float groundHorizontalSpeed;
    public float inAirHorizontalSpeed;
    public float jumpForce;
    public float wallJumpForce;
    public float wallJumpAngle;
    public float wallStickiness;
    public float wallStickDuration;
    public LevelManager levelManager;
    public InputController inputController;
    public CircleCollider2D bottomCollider;
    public CircleCollider2D leftCollider;
    public CircleCollider2D topCollider;
    public CircleCollider2D rightCollider;

    // Private component vars
    private Rigidbody2D rigidbody;

    // private state vars
    bool canJump, shouldJump;
    public void SetShouldJump(bool value)
    {
        shouldJump = value;
    }
    public bool GetShouldJump()
    {
        return shouldJump;
    }
    public bool GetCanJump()
    {
        return canJump;
    }

    bool canFlip, shouldFlip;
    public void SetShouldFlip(bool value)
    {
        shouldFlip = value;
    }
    public void SetCanFlip(bool value)
    {
        canFlip = value;
    }
    public bool GetCanFlip()
    {
        return canFlip;
    }

    bool shouldReset;
    public void SetShouldReset(bool value)
    {
        shouldReset = value;
    }
    float horizontalMove;
    public void SetHorizontalMove(float value)
    {
        horizontalMove = value;
    }

    bool inAir;
    LayerMask terrain;

    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
        canFlip = false;
        shouldReset = false;
        terrain = LayerMask.GetMask("terrain");
        rigidbody = GetComponent<Rigidbody2D>();
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
            Jump();
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
        if (bottomCollider.IsTouchingLayers(terrain))
        {
            StopHorizontalVelocity();
            StopVerticalVelocity();
            canJump = true;
            inAir = false;
        }
        // If the left edge collided with terrain
        if (leftCollider.IsTouchingLayers(terrain))
        {
            StopHorizontalVelocity();
            if (!bottomCollider.IsTouchingLayers(terrain))
            {
                canJump = true;
            }
        }
        // If the right edge collided with terrain
        if (rightCollider.IsTouchingLayers(terrain))
        {
            StopHorizontalVelocity();
            if (!bottomCollider.IsTouchingLayers(terrain))
            {
                canJump = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!bottomCollider.IsTouchingLayers(terrain))
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

    private void Jump()
    {
        //Cancel Player's Current Vertical velocity
        StopVerticalVelocity();
        Vector2 jumpVector = new Vector2(0, 0);
        // If player is on ground, set jump vector normally
        if (!inAir)
        {
            jumpVector.y = jumpForce;
        }
        else
        {
            jumpVector.y = wallJumpForce*Mathf.Sin(wallJumpAngle * Mathf.Deg2Rad);
            if (leftCollider.IsTouchingLayers(terrain))
            {
                jumpVector.x = wallJumpForce * Mathf.Cos(wallJumpAngle * Mathf.Deg2Rad);
            }
            if (rightCollider.IsTouchingLayers(terrain))
            {
                jumpVector.x = -1 * wallJumpForce * Mathf.Cos(wallJumpAngle * Mathf.Deg2Rad);
            }
        }

        rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
        SetShouldJump(false);
        canJump = false;
    }

    public void FlipSuccess()
    {
        canFlip = false;
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

    private void StopVerticalVelocity()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
    }

    private void StopHorizontalVelocity()
    {
        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
    }

}
