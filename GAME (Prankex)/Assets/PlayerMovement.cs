using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float MoveSpeed = 9;
    [SerializeField] private float acceleration = 13;
    [SerializeField] private float deceleration = 16;
    [SerializeField] private float velPower = 0.96f;
    [SerializeField] private float JumpForce = 13;
    [SerializeField] private float fallGravityMultiplier = 2;
    [SerializeField] private float jumpCutMultiplier = 0.4f;
    
    [SerializeField] private LayerMask jumpableGround;

    private bool isJumping;
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
        var jumpInputReleased = Input.GetButtonUp("Jump");

        if (jumpInput && isGrounded()) 
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            isJumping = true;

        }

        if (isGrounded())
        {
            isJumping = false;
        }

        if (dirX < -0.01)
        {
            sprite.flipX = true;
        }
        else if (dirX > 0.01)
        {
            sprite.flipX = false;
        }

        #region Jump Gravity
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
        #endregion

        #region JumpCut
        if(jumpInputReleased && rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }

        #endregion
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

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        
    }
}