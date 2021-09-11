using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public vars
    // Speeds
    public float groundHorizontalSpeed;
    public float inAirHorizontalSpeed;
    public float wallSlidingSpeed;

    // Forces
    public float jumpForce;
    public float wallJumpForce;
    public float wallJumpAngle;

    // Times
    public float wallJumpTime;
    public float moveLockOnWallJump;

    // Sizes
    public float checkWidth;

    // Objects
    public LevelManager levelManager;
    public InputController inputController;
    public Transform bottomCheck;
    public Transform frontCheck;

    // Private vars
    // state vars
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

    bool isFacingRight;
    bool isFrontTouchingWall;
    bool wallSliding;
    bool wallJumping;
    bool inAir;

    // Time Vars
    float moveLockedTime;
    public float GetMoveLockedTime()
    {
        return moveLockedTime;
    }
    public bool IsPlayerLocked()
    {
        if (moveLockedTime > 0f)
        {
            return true;
        }
        return false;
    }
    public void SetMoveLockedTime(float value)
    {
        moveLockedTime = value;
    }

    // Objects
    private Rigidbody2D rb2d;
    LayerMask terrain;

    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
        canFlip = false;
        shouldReset = false;
        wallSliding = false;
        isFacingRight = true;
        moveLockedTime = 0f;
        terrain = LayerMask.GetMask("terrain");
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DoTerrainChecks();
        if (!inAir || isFrontTouchingWall)
        {
            canJump = true;
        }
        wallSliding = (isFrontTouchingWall && inAir && (horizontalMove != 0));

        // Tick down movement lock time
        if (moveLockedTime > 0f)
        {
            moveLockedTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        float speed;
        if ((horizontalMove > 0f && !isFacingRight) ||
            horizontalMove < 0f && isFacingRight)
        {
            ReverseFacing();
        }
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
        if (wallJumping)
        {
            Vector2 jumpVector = new Vector2(0, 0);
            jumpVector.y = wallJumpForce * Mathf.Sin(wallJumpAngle * Mathf.Deg2Rad);
            jumpVector.x = wallJumpForce * Mathf.Cos(wallJumpAngle * Mathf.Deg2Rad);
            if (isFacingRight)
            {
                jumpVector.x = -jumpVector.x;
            }
            rb2d.velocity = jumpVector;
        }

        if (IsPlayerLocked()) return;
        if (!inAir)
        {
            speed = groundHorizontalSpeed;
        }
        else
        {
            speed = inAirHorizontalSpeed;
        }
        if (horizontalMove != 0)
        {
            float xVel = (horizontalMove * speed);
            float yVel = rb2d.velocity.y;
            if (wallSliding)
            {
                xVel = 0;
                yVel = Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue);
            }
            rb2d.velocity = new Vector2(xVel, yVel);
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
        if (IsPlayerLocked()) return;
        //Cancel Player's Current Vertical velocity
        // If player is on ground, set jump vector normally
        if (!inAir)
        {
            Vector2 jumpVector = new Vector2(0, 0);
            StopVerticalVelocity();
            jumpVector.y = jumpForce;
            rb2d.velocity = jumpVector;
        }
        else if(wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpToFalse", wallJumpTime);
            SetMoveLockedTime(moveLockOnWallJump);
        }
        //rb2d.AddForce(jumpVector, ForceMode2D.Impulse);
        SetShouldJump(false);
        canJump = false;
    }

    public void FlipSuccess()
    {
        canFlip = false;
    }

    private void ReverseFacing()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        isFacingRight = !isFacingRight;
    }
    
    private void SetWallJumpToFalse()
    {
        wallJumping = false;
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
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
    }

    private void StopHorizontalVelocity()
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }

    private void DoTerrainChecks()
    {
        // Check Contact points for touching terrain
        inAir = !Physics2D.OverlapBox(bottomCheck.position, new Vector2( GetComponent<Collider2D>().bounds.size.x * .9f, checkWidth), 0, terrain);
        isFrontTouchingWall = Physics2D.OverlapBox(frontCheck.position, new Vector2(checkWidth, GetComponent<Collider2D>().bounds.size.y * .9f), 0, terrain);
    }

}
