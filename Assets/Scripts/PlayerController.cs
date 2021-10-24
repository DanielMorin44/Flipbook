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
    public float maxSlopeAngle;

    // Physics Materials
    public PhysicsMaterial2D noFriction;
    public PhysicsMaterial2D fullFriction;

    // Times
   // public float wallJumpTime;
    public float moveLockOnWallJump;
    public float coyoteTime;

    // Sizes
    public float checkWidth;
    public float slopeCheckDistance;

    // Objects
    private LevelManager levelManager;
    private InputController inputController;

    // Private vars
    // state vars
    bool canWallJump, canRegularJump, shouldJump, jumping;
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
        return canRegularJump || canWallJump;
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

    bool holdForWallSlide = false;
    public void SetHoldForWallSlide(bool value)
    {
        holdForWallSlide = value;
    }
    public void ToggleWallSlide()
    {
        holdForWallSlide = !holdForWallSlide;
    }

    bool isFacingRight;
    float facing;
    bool isFrontTouchingWall;
    bool wallSliding;
    bool wallJumping;
    bool inAir;

    // Private Collection vars
    private int numKeys;

    // Private slope vars
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;
    private Vector2 slopeNormalPerp;
    private bool isOnSlope;
    private bool canWalkOnSlope;
    private bool sliding;
    private float wallAngle = 87;

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
    float coyoteTimeRemaining;
    float wallJumpCoyoteTimeRemaining;
    bool jumpedSinceLanded = true;
    bool wallJumpSinceWallTouch = true;
    bool wasFacingRightOnLastWallTouch;

    // Objects
    private Rigidbody2D rb2d;
    private CircleCollider2D circle;
    private LayerMask terrain;

    // Start is called before the first frame update
    void Start()
    {
        canWallJump = false;
        canRegularJump = false;
        canFlip = false;
        shouldReset = false;
        wallSliding = false;
        isFacingRight = true;
        numKeys = 0;
        facing = 1;
        moveLockedTime = 0f;
        coyoteTimeRemaining = coyoteTime;
        terrain = LayerMask.GetMask("terrain");
        rb2d = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        inputController = GameObject.FindObjectOfType<InputController>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    #region update functions
    void Update()
    {
        DoTerrainChecks();
        DoSlopeCheck();
        canWallJump = wallJumpCoyoteTimeRemaining > 0f && !wallJumpSinceWallTouch;
        canRegularJump = (coyoteTimeRemaining > 0f && !jumpedSinceLanded);
        if (sliding) { canWallJump = false; canRegularJump = false; }
        wallSliding = (isFrontTouchingWall && inAir && !sliding && holdForWallSlide);

        // Tick down lock times
        if (moveLockedTime > 0f)
        {
            if (isFrontTouchingWall && moveLockedTime < (moveLockOnWallJump - .1))
            {
                moveLockedTime = 0.0f;
            }
            moveLockedTime -= Time.deltaTime;
            if(moveLockedTime < 0f)
            {
                horizontalMove = 0.0f;
            }
        }
        if(coyoteTimeRemaining > 0f && inAir)
        {
            coyoteTimeRemaining -= Time.deltaTime;
        }
        if (wallJumpCoyoteTimeRemaining > 0f && !isFrontTouchingWall)
        {
            wallJumpCoyoteTimeRemaining -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
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
        DoMovement();
    }

    private void DoTerrainChecks()
    {
        // Check Contact points for touching terrain
        inAir = !Physics2D.OverlapBox(new Vector2(circle.bounds.center.x, circle.bounds.min.y), new Vector2(circle.bounds.size.x * .8f, checkWidth), 0, terrain);
        if (!inAir)
        {
            jumpedSinceLanded = false;
            coyoteTimeRemaining = coyoteTime;
        }
        sliding = (slopeSideAngle > maxSlopeAngle);
        isFrontTouchingWall = Physics2D.OverlapBox(new Vector2(circle.bounds.center.x + (circle.bounds.extents.x * facing), circle.bounds.center.y), new Vector2(checkWidth, circle.bounds.size.y * .8f), 0, terrain);
        if (isFrontTouchingWall)
        {
            wasFacingRightOnLastWallTouch = isFacingRight;
            wallJumpSinceWallTouch = false;
            wallJumpCoyoteTimeRemaining = coyoteTime;
        }
    }

    private void DoSlopeCheck()
    {
        // Todo: figure out why there is this magic value and remove
        Vector2 checkpos = transform.position - new Vector3(0.0f, circle.radius + .35f);
        HorizontalSlopeCheck(checkpos);
        VerticalSlopeCheck(checkpos);
    }

    private void HorizontalSlopeCheck(Vector2 checkpos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkpos, transform.right, slopeCheckDistance, terrain);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkpos, -transform.right, slopeCheckDistance, terrain);
        if (slopeHitFront)
        {
            if (Vector2.Angle(slopeHitFront.normal, Vector2.up) < wallAngle)
            {
                slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
                isOnSlope = true;
            }
        } else  if (slopeHitBack)
        {
            if (Vector2.Angle(slopeHitBack.normal, Vector2.up) < wallAngle)
            {
                slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
                isOnSlope = true;
            }
        } else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }
    }

    private void VerticalSlopeCheck(Vector2 checkpos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkpos, Vector2.down, slopeCheckDistance, terrain);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeDownAngle != slopeDownAngleOld)
            {
                isOnSlope = true;
            }
            slopeDownAngleOld = slopeDownAngle;
        }
        canWalkOnSlope = (!(slopeDownAngle > maxSlopeAngle) && !(slopeSideAngle > maxSlopeAngle));
        rb2d.sharedMaterial = (isOnSlope && horizontalMove == 0.0f && canWalkOnSlope) ? fullFriction : noFriction;
    }
    #endregion

    #region movement functions

    private void DoMovement()
    {
        if (IsPlayerLocked()) return;
        if ((horizontalMove > 0f && !isFacingRight) ||
            horizontalMove < 0f && isFacingRight)
        {
            ReverseFacing();
        }
        float speed = inAir ? inAirHorizontalSpeed : groundHorizontalSpeed;
        if (inAir)
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
        else if (isOnSlope && canWalkOnSlope)
        {
            float xVel = ( slopeNormalPerp.x * -horizontalMove * speed);
            float yVel = ( slopeNormalPerp.y * -horizontalMove * speed);
            rb2d.velocity = new Vector2(xVel, yVel);
        }
        else if (!isOnSlope)
        {
           float xVel = (horizontalMove * speed);
           float yVel = 0.0f;
           rb2d.velocity = new Vector2(xVel, yVel);
        }
    }

    private void Jump()
    {
        if (IsPlayerLocked()) return;
        //Cancel Player's Current Vertical velocity
        StopVerticalVelocity();
        if (!inAir)
        {
            // If player is not wall jumping, set jump vector normally
            Vector2 jumpVector = new Vector2(rb2d.velocity.x, 0);
            jumpVector.y = jumpForce;
            rb2d.velocity = jumpVector;
            jumpedSinceLanded = true;
        }
        else if(canWallJump)
        {
            if (isFacingRight == wasFacingRightOnLastWallTouch)
            {
                ReverseFacing();
            }
            rb2d.velocity = new Vector2(facing * wallJumpForce * Mathf.Cos(wallJumpAngle * Mathf.Deg2Rad),
                            wallJumpForce * Mathf.Sin(wallJumpAngle * Mathf.Deg2Rad));
            SetMoveLockedTime(moveLockOnWallJump);
        } else
        { // Player jumped during coyote time
          // If player is not wall jumping, set jump vector normally
            Vector2 jumpVector = new Vector2(rb2d.velocity.x, 0);
            jumpVector.y = jumpForce;
            rb2d.velocity = jumpVector;
            jumpedSinceLanded = true;
        }
        SetShouldJump(false);
        canWallJump = false;
        canRegularJump = false;
    }

    private void StopVerticalVelocity()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
    }

    private void StopHorizontalVelocity()
    {
        rb2d.velocity = new Vector2(0, rb2d.velocity.y);
    }

    private void ReverseFacing()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        isFacingRight = !isFacingRight;
        facing *= -1;
    }
    
    #endregion

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

    public void Kill()
    {
        levelManager.PlayerDied();
    }

    private void ResetPlayer()
    {
        SetShouldReset(false);
        Kill();
    }

    private void OnDrawGizmosSelected()
    {
        if (circle)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(new Vector2(circle.bounds.center.x, circle.bounds.min.y), new Vector2(circle.bounds.size.x * .8f, checkWidth));
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector2(circle.bounds.center.x + (circle.bounds.extents.x * facing), circle.bounds.center.y), new Vector2(checkWidth, circle.bounds.size.y * .8f));
        }
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
