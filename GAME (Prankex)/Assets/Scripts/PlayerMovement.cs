using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region declarations
    public PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    public Transform PlayerTransform;

    private float movement;
    private float horizontal;
    private float vertical;
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

    private bool CanDash = true;
    private bool IsDashing;
    private float DashingTime = 0.2f;
    private float DashingCooldown = 1f;

    private float gravityScale = 1.1f;

    
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private enum MovementState { idle, running, jumping, falling, rolling, sliding }

    #endregion

    

    void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        tr.emitting = false;

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.started += Jump;
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Jump.canceled += Jump;
        playerInputActions.Player.Move.performed += Move;
        playerInputActions.Player.Move.canceled += Move;
        playerInputActions.Player.Dash.started += Dash;

    }

    private void Update()
    {
        if (IsDashing)
        {
            rb.gravityScale = 0f;
        }
        

        if (!isWallJumping)
        {
            Flip();
        }
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

        UpdateAnimationState();
    }
    private void FixedUpdate()
    {
        // Application of the Run Method
        if (!IsDashing)
        {
            float targetSpeed = horizontal * MoveSpeed;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        // Getting the input value
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
        Debug.Log(context);

        // Stop the player when no input value
        if (context.canceled)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void Flip()
    {
        if (horizontal < -0.01 && isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isFacingTheRight = false;
        }
        else if (horizontal > 0.01 && !isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            isFacingTheRight = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround); 
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (IsGrounded())
        {
            isJumping = false;
        }
        // Jump when pressed
        if (context.started && IsGrounded())
        {
            isJumping = true;
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }

        // Wall jump
        if (context.started && wallJumpingCounter > 0f) // If pressed jump and player is still wall jumping (0s < counter < 0.2s)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingTheRight = !isFacingTheRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
        // Jump cut when finished
        if (context.canceled)
        {
            isJumping = false;
        }
        if (context.canceled && rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }

    private bool IsWalled() 
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        //remove horizontal!=0f if you want automatic slide
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;

            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
        else
        {
            isWallSliding = false;
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
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    public void Dash(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (CanDash && context.started)
        {
            StartCoroutine(Dashing());
        }
        
    }

    // Dash method with enumerator method executed in the input action as coroutine
    private IEnumerator Dashing()
    {
        CanDash = false;
        IsDashing = true;
        float originalGravity = rb.gravityScale;
        Vector2 ColliderSize = coll.size;
        //cancel velocity before dash to avoid inertia
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0f;

        // right
        if (horizontal > 0.01 & vertical == 0)
        { rb.velocity = new Vector2(DashForce, 0f); }

        // up right
        else if (horizontal > 0.01 & vertical > 0.01)
        { rb.velocity = new Vector2(DashForce / 2, DashForce / 2); }

        // up
        else if (horizontal == 0 & vertical > 0.01)
        { rb.velocity = new Vector2(0f, DashForce / 2); }

        // up left
        else if (horizontal < -0.01 & vertical > 0.01)
        { rb.velocity = new Vector2(-DashForce / 2, DashForce / 2); }

        // left
        else if (horizontal < -0.01 & vertical == 0)
        { rb.velocity = new Vector2(-DashForce, 0f); }

        // down left
        else if (horizontal < -0.01 & vertical < 0.01)
        { rb.velocity = new Vector2(-DashForce / 2, -DashForce / 2); }

        // down
        else if (horizontal == 0 & vertical < 0.01)
        { rb.velocity = new Vector2(0f, -DashForce); }

        // down right
        else if (horizontal > 0.01 & vertical < 0.01)
        { rb.velocity = new Vector2(DashForce / 2, -DashForce / 2); }

        tr.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        tr.emitting = false;

        rb.gravityScale = originalGravity;
        IsDashing = false;
        yield return new WaitForSeconds(DashingCooldown);
        CanDash = true;
    }
    private void UpdateAnimationState()
    {
        MovementState state;
        if (horizontal > 0f)
        {
            state = MovementState.running;
        }
        else if (horizontal < 0f)
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