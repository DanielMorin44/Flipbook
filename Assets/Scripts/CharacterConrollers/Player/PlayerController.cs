using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    // This 'split-screen' config option is gross and should be replaced with some inheritance model
    public bool isSplitScreen;
    private bool sideA;
    public TilemapCollider2D sideAMap;
    public TilemapCollider2D sideBMap;
    public GameObject playerShadow;

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
    protected LevelManager levelManager;
    protected InputController inputController;

    // Animations
    public Animator anim;
    public Animator shadowAnimator;

    //Sounds
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    // Private vars
    // state vars
    protected bool canWallJump, canRegularJump, shouldJump, jumping;
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

    protected bool canFlip, shouldFlip;
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

    protected bool shouldReset;
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

    protected bool isFacingRight;
    protected float facing;
    bool isFrontTouchingWall;
    protected bool wallSliding;
    bool wallJumping;
    bool inAir;
    int hasCoin = -1;

    // Private Collection vars
    protected int numKeys;

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
    protected float moveLockedTime;
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
    protected float coyoteTimeRemaining;
    float wallJumpCoyoteTimeRemaining;
    bool jumpedSinceLanded = true;
    bool wallJumpSinceWallTouch = true;
    bool wasFacingRightOnLastWallTouch;

    // Objects
    protected Rigidbody2D rb2d;
    protected CircleCollider2D circle;
    protected BoxCollider2D box;
    protected LayerMask terrain;

    // Start is called before the first frame update
    void Start()
    {
        canWallJump = false;
        canRegularJump = false;
        canFlip = isSplitScreen;
        shouldReset = false;
        wallSliding = false;
        isFacingRight = true;
        numKeys = 0;
        facing = 1;
        moveLockedTime = 0f;
        coyoteTimeRemaining = coyoteTime;
        terrain = isSplitScreen ? LayerMask.GetMask("SideA") : LayerMask.GetMask("terrain");
        if(sideBMap != null) sideBMap.enabled = false;
        sideA = true;
        rb2d = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        circle = GetComponent<CircleCollider2D>();
        inputController = GameObject.FindFirstObjectByType<InputController>();
        levelManager = GameObject.FindFirstObjectByType<LevelManager>();
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
            if (moveLockedTime < 0f)
            {
                horizontalMove = 0.0f;
            }
        }
        if (coyoteTimeRemaining > 0f && inAir)
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

        // Reset state variables due to landing / jamping / falling
        if (!inAir)
        {
            jumpedSinceLanded = false;
            coyoteTimeRemaining = coyoteTime;

            //animations
            setAnimationVal("inAir", false);
            setAnimationVal("isFalling", false);
        }
        else
        {
            //animations
            setAnimationVal("inAir", true);

            if (rb2d.linearVelocity.y <= 0)
            {
                //animations
                setAnimationVal("isFalling", true);
            }

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
        }
        else if (slopeHitBack)
        {
            if (Vector2.Angle(slopeHitBack.normal, Vector2.up) < wallAngle)
            {
                slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
                isOnSlope = true;
            }
        }
        else
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
            if (slopeDownAngle != slopeDownAngleOld)
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

        //Animation and audio checks
        if (horizontalMove == 0 && !inAir)
        {
            setAnimationVal("isRunning", false);
        }
        else if (!inAir)
        {
            setAnimationVal("isRunning", true);
        }

        if (wallSliding)
        {
            anim.SetBool("isWallsliding", true);
        }
        else
        {
            anim.SetBool("isWallsliding", false);
        }

            //Movement
            float speed = inAir ? inAirHorizontalSpeed : groundHorizontalSpeed;
        if (inAir)
        {
            float xVel = (horizontalMove * speed);
            float yVel = rb2d.linearVelocity.y;
            if (wallSliding)
            {
                xVel = 0;
                yVel = Mathf.Clamp(rb2d.linearVelocity.y, -wallSlidingSpeed, float.MaxValue);
            }

                rb2d.linearVelocity = new Vector2(xVel, yVel);
        }
        else if (isOnSlope && canWalkOnSlope)
        {
            float xVel = (slopeNormalPerp.x * -horizontalMove * speed);
            float yVel = (slopeNormalPerp.y * -horizontalMove * speed);
            rb2d.linearVelocity = new Vector2(xVel, yVel);
        }
        else if (!isOnSlope)
        {
            float xVel = (horizontalMove * speed);
            float yVel = 0.0f;
            rb2d.linearVelocity = new Vector2(xVel, yVel);
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
            Vector2 jumpVector = new Vector2(rb2d.linearVelocity.x, 0);
            jumpVector.y = jumpForce;
            rb2d.linearVelocity = jumpVector;
            jumpedSinceLanded = true;
        }
        else if (canWallJump)
        {
            if (isFacingRight == wasFacingRightOnLastWallTouch)
            {
                ReverseFacing();
            }
            rb2d.linearVelocity = new Vector2(facing * wallJumpForce * Mathf.Cos(wallJumpAngle * Mathf.Deg2Rad),
                            wallJumpForce * Mathf.Sin(wallJumpAngle * Mathf.Deg2Rad));
            SetMoveLockedTime(moveLockOnWallJump);

        } else
        { // Player jumped during coyote time
          // If player is not wall jumping, set jump vector normally
            Vector2 jumpVector = new Vector2(rb2d.linearVelocity.x, 0);
            jumpVector.y = jumpForce;
            rb2d.linearVelocity = jumpVector;
            jumpedSinceLanded = true;

        }
        SetShouldJump(false);
        canWallJump = false;
        canRegularJump = false;

        //Set animation triggers
        anim.SetBool("isFalling", false);
        anim.SetBool("isWallsliding", false);
        anim.SetTrigger("jumped");
    }

    private void StopVerticalVelocity()
    {
        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0);
    }

    private void StopHorizontalVelocity()
    {
        rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
    }

    private void ReverseFacing()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        isFacingRight = !isFacingRight;
        facing *= -1;
    }

    #endregion

    private bool CheckSplitScreenFlipAllowed()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(box.bounds.center,
                                                        new Vector2(box.bounds.size.x, box.bounds.size.y * .9f),
                                                        0,
                                                        sideA ? LayerMask.GetMask("SideA") : LayerMask.GetMask("SideB"));

        if (colliders.Length != 0)
        {
            Debug.Log("We collided");
            return false;
        }
        return true;
        /*for (int i = 0; i < colliders.Length; i++)
        {
            Component collider = flipCandidates[index].GetComponentInChildren(typeof(Grid)).GetComponentInChildren(typeof(CompositeCollider2D));
            if (colliders[i].gameObject.name == collider.gameObject.name)
            {
                flipCandidates[index].SetActive(false);
                return false;
            }
        }*/
    }

    public void handleSplitScreenFlip()
    {
        if (CheckSplitScreenFlipAllowed())
        {
            Debug.Log("We Flipped!");
            sideA = !sideA;
            if (sideA)
            {
                terrain = LayerMask.GetMask("SideA");
                gameObject.layer = LayerMask.NameToLayer("SideAPlayer");
                playerShadow.layer = LayerMask.NameToLayer("SideBPlayer");
                sideAMap.enabled = true;
                sideBMap.enabled = false;
            } else
            {
                terrain = LayerMask.GetMask("SideB");
                gameObject.layer = LayerMask.NameToLayer("SideBPlayer");
                playerShadow.layer = LayerMask.NameToLayer("SideAPlayer");
                sideBMap.enabled = true;
                sideAMap.enabled = false;
            }
        }
    }

    private void Flip()
    {
        shouldFlip = false;
        if (isSplitScreen)
        {
            handleSplitScreenFlip();
        }
        else
        {
            // CanFlip = true if flip failed
            inputController.InitiateFlip();
        }
    }

    public void FlipSuccess()
    {
        canFlip = isSplitScreen;
        PlayClip("page flip");
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

    public int HoldingCoin()
    {
        return hasCoin;
    }

    public void AddCoin(int id)
    {
        hasCoin = id;
    }

    private void setAnimationVal(string fieldName, bool value)
    {
        anim.SetBool(fieldName, value);
        if (shadowAnimator != null)
        {
            shadowAnimator.SetBool(fieldName, value);
        }
    }


    //Audio
    public void PlayClip(string clipName)
    {
        int clipToPlay = 0;
        bool continuous = false;

        switch (clipName)
        {
            case "light footstep":
                clipToPlay = 0;
                break;
            case "big footstep":
                clipToPlay = 1;
                break;
            case "jump":
                clipToPlay = 2;
                break;
            case "land":
                clipToPlay = 3;
                break;
            case "gem":
                clipToPlay = 4;
                break;
            case "page flip":
                clipToPlay = 5;
                break;
            case "wallslide":
                clipToPlay = 6;
                continuous = true;
                break;
            default:
                clipToPlay = -1; //safety if there is no string match
                Debug.Log("Sound not found: " + clipName);
                break;
        }


        if (clipToPlay >= 0 && clipToPlay < audioClips.Length)
        {

            if (continuous)
            {
                audioSource.clip = audioClips[clipToPlay];
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(audioClips[clipToPlay]);
            }
        }

    }

    public void StopClip()
    {
        audioSource.clip = null; // don't use audioSource.Stop() because it messes with the OneShots
    }
}
