using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform PlayerTransform;

    [SerializeField] private float MoveSpeed = 9;
    [SerializeField] private float acceleration = 13;
    [SerializeField] private float deceleration = 16;
    [SerializeField] private float velPower = 0.96f;
    [SerializeField] private float JumpForce = 13;
    [SerializeField] private float fallGravityMultiplier = 1.1f;
    [SerializeField] private float jumpCutMultiplier = 0.4f;
    [SerializeField] private float maxGravity = 3;
    [SerializeField] private float DashForce = 200f;
    [SerializeField] private TrailRenderer tr;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;

    [SerializeField] private bool isWalledBool;


    private bool isFacingTheRight = true;
    
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isWallJumping;
    private bool isWallSliding;
    private float wallSlidingSpeed = 1f;

    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8, 16);

    private bool IsDashing;
    private float DashingTime = 0.2f;
    private float DashingCooldown = 1f;
    private Vector2 veloToApply;

    private float dirX;
    private float dirY;
    private float gravityScale = 1.1f;
    
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;


    [SerializeField] private bool rightPressed;
    [SerializeField] private bool leftPressed;
    [SerializeField] private bool upPressed;
    [SerializeField] private bool downPressed;
    [SerializeField] private bool uprightPressed;
    [SerializeField] private bool upleftPressed;
    [SerializeField] private bool downrightPressed;
    [SerializeField] private bool downleftPressed;
    [SerializeField] private bool CanDash = true;
    [SerializeField] private bool isGrounded = true;
    private enum directions { up, upright, right, downright, down, downleft, left,upleft };
    private directions direction;
    private enum MovementState { idle, running, jumping, falling, rolling, sliding }

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        tr.emitting = false;
        
    }
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        var jumpInput = Input.GetButtonDown("Jump");

        //z,q,s,d pressed control
        CheckDirections();

        if (IsGrounded())
        {
            isJumping = false;
            CanDash = true;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }



        if (jumpInput && IsGrounded())
        {
            isJumping = true;
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }

        if (!isWallJumping)
        {
            Flip();
        }
        JumpCut();
        JumpGravity();
        WallSlide();
        WallJump();
        if (IsWalled())
        {
            isWalledBool = true;
        }
        else
        {
            isWalledBool = false;
        }
        if (Input.GetButtonDown("Fire1") && CanDash)
        {
            StartCoroutine(Dash());
        }
        if (IsDashing)
        {
            rb.gravityScale = 0f;
        }
        UpdateAnimationState();
    }

    void FixedUpdate()
    {
        if (!IsDashing)
        {
            #region Run
            float targetSpeed = dirX * MoveSpeed;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);

            #endregion
        }
    }

   
    private void CheckDirections()
    {
        bool qInput = Input.GetKey(KeyCode.Q);
        bool zInput = Input.GetKey(KeyCode.Z);
        bool sInput = Input.GetKey(KeyCode.S);
        bool dInput = Input.GetKey(KeyCode.D);

        if (sInput && !qInput && !zInput && !dInput) { direction = directions.down; downPressed = true; }
        else { downPressed = false; }
        if (sInput && qInput && !zInput && !dInput) { direction = directions.downleft; downleftPressed = true; }
        else { downleftPressed = false; }
        if (!sInput && qInput && !zInput && !dInput) { direction = directions.left; leftPressed = true; }
        else { leftPressed = false; }
        if (!sInput && qInput && zInput && !dInput) { direction = directions.upleft; upleftPressed = true; }
        else { upleftPressed = false; }
        if (!sInput && !qInput && zInput && !dInput) { direction = directions.up; upPressed = true; }
        else { upPressed = false; }
        if (!sInput && !qInput && zInput && dInput) { direction = directions.upright; uprightPressed = true; }
        else { uprightPressed = false; }
        if (!sInput && !qInput && !zInput && dInput) { direction = directions.right; rightPressed = true; }
        else { rightPressed = false; }
        if (sInput && !qInput && !zInput && dInput) { direction = directions.downright; downrightPressed = true; }
        else { downrightPressed = false; }
    }
    private bool IsGrounded()
    {
        
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .3f, jumpableGround);
        
    }

    private bool IsWalled() 
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        //remove dirX!=0f if you want automatic slide
        if (IsWalled() && !IsGrounded() && dirX != 0f)
        {
            isWallSliding = true;

            
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
        else
        {
            isWallSliding = false;
        }
    }






    private void Flip()
    {
        if (dirX < -0.01 && isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isFacingTheRight = false;
        }
        else if (dirX > 0.01 && !isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isFacingTheRight = true;
        }
    }
    private void JumpGravity()
    {
        if (rb.velocity.y < -0.01f)
        {
            isJumping = false;
            //To set minmal move speed
            rb.gravityScale = Mathf.Min(rb.gravityScale * fallGravityMultiplier, maxGravity);
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void JumpCut()
    {
        
        var jumpInputReleased = Input.GetButtonUp("Jump");
        if (jumpInputReleased && rb.velocity.y > 0)
        {
            isJumping = false;
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            //opposite direction
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            //Counter starts, player jumped off the wall
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) // If pressed jump and player is still wall jumping (0s < counter < 0.2s)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingTheRight = !isFacingTheRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private IEnumerator Dash()
    {
     
        float originalGravity = rb.gravityScale;
        Vector2 ColliderSize = coll.size;
        
        //Used to reset veloToApply vector
        veloToApply = new Vector2(0, 0);

        if (direction == directions.right)
        { veloToApply = new Vector2(DashForce * 3 / 4, 0f); }

        else if (direction == directions.upright)
        { veloToApply = new Vector2(DashForce / 2, DashForce / 2); }

        else if (direction == directions.up)
        { veloToApply = new Vector2(0f, DashForce / 2); }

        else if (direction == directions.upleft)
        { veloToApply = new Vector2(-DashForce / 2, DashForce / 2); }

        else if (direction == directions.left)
        { veloToApply = new Vector2(-DashForce * 3 / 4, 0f); }

        else if (direction == directions.downleft && isGrounded == false)
            //If the player is falling, then its gravity velocity to dashforce
            if (rb.velocity.y < 0f) { veloToApply = new Vector2(-DashForce / 2, rb.velocity.y - DashForce / 2); }
            else { veloToApply = new Vector2(-DashForce / 2, -DashForce / 2); }

        else if (direction == directions.down && isGrounded == false)
            //If the player is falling, then its gravity velocity to dashforce
            if (rb.velocity.y < 0f) { veloToApply = new Vector2(0f, rb.velocity.y - DashForce / 2); }
            else { veloToApply = new Vector2(0f, -DashForce / 2); }


        else if (direction == directions.downright && isGrounded == false)
            //If the player is falling, then its gravity velocity to dashforce
            if (rb.velocity.y < 0f) { veloToApply = new Vector2(DashForce/2, rb.velocity.y - DashForce / 2); }
            else { veloToApply = new Vector2(DashForce / 2, -DashForce / 2); }
      
        //Applying velocity vector 
        if(veloToApply != new Vector2(0, 0)) {
            CanDash = false;
            IsDashing = true;


            coll.size = new Vector2((float)0.8855777, (float)0.4);

            //To avoid gravity effect during dash
            rb.gravityScale = 0f;
            //cancel velocity before dash to avoid inertia
            rb.velocity = new Vector2(0, 0);
            rb.velocity = veloToApply;
        }
       

        tr.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        //Cancel velocity after dash
        //rb.velocity = new Vector2(0, 0);
        tr.emitting = false;
        coll.size = ColliderSize; 
        rb.gravityScale = originalGravity;
        IsDashing = false;
        //in case the player is falling and cannot touch any ground/wall, DashingCooldown
        yield return new WaitForSeconds(DashingCooldown);
        CanDash = true;
    }
    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
        }
        else
        {
            state = MovementState.idle;
        }

        if (isJumping == true || isWallJumping == true)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f && !isWallSliding)
        {
            state = MovementState.falling;
        }
        else if (IsDashing == true)
        {
            state = MovementState.rolling;
        }
        else if (isWallSliding == true)
        {
            state = MovementState.sliding;
        }
        anim.SetInteger("state", (int)state);
    }
}