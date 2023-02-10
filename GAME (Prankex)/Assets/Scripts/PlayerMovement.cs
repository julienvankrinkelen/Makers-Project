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

    private bool IsDashing;
    [SerializeField] private float DashingTime = 0.15f;
    private float DashingCooldown = 1f;
    private Vector2 veloToApply;

   
    private float gravityScale = 2.5f;
    
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;


    
    [SerializeField] private bool CanDash = true;
    [SerializeField] private bool isGrounded = true;
   
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
        

        if (!isWallJumping)
        {
            Flip();
        }
        JumpGravity();
        WallSlide();
        WallJump();

        if (IsWalled()) {isWalledBool = true; }
        else{ isWalledBool = false;}

      
        if (IsDashing){rb.gravityScale = 0f; }

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
        

        // Stop the player when no input value
        if (context.canceled)
        {
            //ATTENTION A CES DEUX LIGNE//
            //-> si on met vélocité à zero sur l'axe X, on aura un effet très dynamique et sec qui annule le "flou" de mouvement créé par Accel Decel de fixedupdate
            //-> si on met vélocité à zero sur l'axe Y, on peut annuler la vélocité induite de la gravité en straffant gauche droite lors d'une chute


            //rb.gravityScale = 0f;
            //rb.velocity = new Vector2(0, 0);
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
        float originalGravity = rb.gravityScale;
        
        
        //Used to reset veloToApply vector
        veloToApply = new Vector2(0, 0);

        // right
        if (horizontal > 0.01 && vertical == 0)
        { veloToApply = new Vector2(DashForce * 3 / 4, 0f); }

        // up right
        else if (horizontal > 0.01 && vertical > 0.01)
        { veloToApply = new Vector2(DashForce / 2, DashForce / 2); }

        // up
        else if (horizontal == 0 && vertical > 0.01)
        { veloToApply = new Vector2(0f, DashForce / 2); }

        // up left
        else if (horizontal < -0.01 && vertical > 0.01)
        { veloToApply = new Vector2(-DashForce / 2, DashForce / 2); }

        // left
        else if (horizontal < -0.01 && vertical == 0) 
        { veloToApply = new Vector2(DashForce * -3/4, 0f); }

        // down left
        else if (horizontal < -0.01 && vertical < 0.01 && isGrounded == false)
        //If the player is falling, then its gravity velocity to dashforce
            if (rb.velocity.y < 0f) { veloToApply = new Vector2(-DashForce / 2, rb.velocity.y - DashForce / 2); }
            else { veloToApply = new Vector2(-DashForce / 2, -DashForce / 2); }

        // down
        else if (horizontal == 0 && vertical < 0.01 && isGrounded == false)
        //If the player is falling, then its gravity velocity to dashforce
            if (rb.velocity.y < 0f) { veloToApply = new Vector2(0f, rb.velocity.y - DashForce / 2); }
            else { veloToApply = new Vector2(0f, -DashForce / 2); }

        // down right
        else if (horizontal > 0.01 && vertical < 0.01 && isGrounded == false)
        //If the player is falling, then its gravity velocity to dashforce
            if (rb.velocity.y < 0f) { veloToApply = new Vector2(DashForce/2, rb.velocity.y - DashForce / 2); }
            else { veloToApply = new Vector2(DashForce / 2, -DashForce / 2); }

        if (veloToApply != new Vector2(0, 0)) {
            IsDashing = true;

            //To avoid gravity effect during dash
            rb.gravityScale = 0f;
            //cancel velocity before dash to avoid inertia
            rb.velocity = new Vector2(0, 0);
            rb.velocity = veloToApply;
        }

        tr.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        //Reduce drastically velocity after dash
        rb.velocity = new Vector2(rb.velocity.x/3, rb.velocity.y/3);
        tr.emitting = false;

        rb.gravityScale = originalGravity;
        IsDashing = false;
        //in case the player is falling and cannot touch any ground/wall, DashingCooldown
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