using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallCheck;

    [SerializeField] private bool isWalledBool;


    private bool isFacingTheRight = true;
    
    private bool isJumping;
    private bool isWallJumping;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8, 16);


    private float dirX;
    private float gravityScale = 1.1f;
    
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        
    }
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        var jumpInput = Input.GetButtonDown("Jump");

        if (jumpInput && IsGrounded()) 
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            isJumping = true;

        }

        if (IsGrounded())
        {
            isJumping = false;
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

        
    }

    void FixedUpdate()
    {
      
            #region Run
            float targetSpeed = dirX * MoveSpeed;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);

            #endregion
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        
    }

    private bool IsWalled() 
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        if (IsWalled() && !IsGrounded() && dirX!=0f)
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
}