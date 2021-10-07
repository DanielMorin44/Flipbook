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
    public float stickAmnt;

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
    bool canJump, shouldJump, jumping;
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
    float facing;
    bool isFrontTouchingWall;
    bool wallSliding;
    bool wallJumping;
    bool inAir;

    // Private Collection vars
    private int numKeys;

    // Time Vars
    float moveLockedTime;
    public float GetMoveLockedTime()
    {
        return moveLockedTime;
    }
    public bool IsPlayerLocked()
    {
        return (moveLockedTime > 0f);
    }
    public void SetMoveLockedTime(float value)
    {
        moveLockedTime = value;
    }

    // Objects
    private Rigidbody2D rb2d;
    public BoxCollider2D box;
    LayerMask terrain;

    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
        canFlip = false;
        shouldReset = false;
        wallSliding = false;
        isFacingRight = true;
        numKeys = 0;
        facing = 1;
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
        wallSliding = (isFrontTouchingWall && inAir);

        // Tick down lock times
        if (moveLockedTime > 0f)
        {
            if (wallSliding && moveLockedTime < (moveLockOnWallJump - .1))
            {
                moveLockedTime = 0.0f;
            }
            moveLockedTime -= Time.deltaTime;
            if(moveLockedTime < 0f)
            {
                horizontalMove = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        float speed;
        if (shouldReset)
        {
            ResetPlayer();
            return;
        }
        if (shouldJump)
        {
            Jump();
            return;
        }
        if (shouldFlip)
        {
            Flip();
        }
        if (wallJumping)
        {
            rb2d.velocity = new Vector2( -facing * wallJumpForce * Mathf.Cos(wallJumpAngle * Mathf.Deg2Rad), 
                                        wallJumpForce * Mathf.Sin(wallJumpAngle * Mathf.Deg2Rad));
        }
        if (!inAir && horizontalMove == 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * (1- stickAmnt), rb2d.velocity.y);
        }
        if (IsPlayerLocked()) return;
        if ((horizontalMove > 0f && !isFacingRight) ||
            horizontalMove < 0f && isFacingRight)
        {
            ReverseFacing();
        }
        speed = inAir ? inAirHorizontalSpeed : groundHorizontalSpeed;
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
            Vector2 jumpVector = new Vector2(rb2d.velocity.x, 0);
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
        facing *= -1;
    }
    
    private void SetWallJumpToFalse()
    {
        wallJumping = false;
        ReverseFacing();
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
        inAir = !Physics2D.OverlapBox(new Vector2(box.bounds.center.x, box.bounds.min.y), new Vector2( box.bounds.size.x * .8f, checkWidth), 0, terrain);
        isFrontTouchingWall = Physics2D.OverlapBox(new Vector2(box.bounds.center.x + (box.bounds.extents.x * facing), box.bounds.center.y), new Vector2(checkWidth, box.bounds.size.y * .8f), 0, terrain);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(new Vector2(box.bounds.center.x, box.bounds.min.y), new Vector2(box.bounds.size.x * .8f, checkWidth));
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector2(box.bounds.center.x + (box.bounds.extents.x * facing), box.bounds.center.y), new Vector2(checkWidth, box.bounds.size.y * .8f));
    }

    public bool TryUnlock()
    {
        if (numKeys > 0)
        {
            numKeys--;
            return true;
        }
        return false;
    }

    public void AddKey(int value)
    {
        numKeys += value;
    }

    public int GetNumKeys()
    {
        return numKeys;
    }
}
