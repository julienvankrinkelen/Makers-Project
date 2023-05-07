using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    
    private PlayerInputActions playerInputActions;
    public Transform PlayerTransform;

    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    public LayerMask enemyLayers;
    public Transform attackPoint;

    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    bool BombSelected = false;
    bool CandleSelected = true;

    public int PlayerHealth = 4;
    public int CurrentHealth;
    [SerializeField] private float DamageForce = 13;
    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        CurrentHealth = PlayerHealth;
    }
    private void Awake()
    {

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Slash.performed += Slash;

    }

    public void Slash(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextAttackTime)
        {
            // animation trigger
            anim.SetTrigger("slash");

            nextAttackTime = Time.time + 1f / attackRate;

        }

        if (context.canceled)
        {
            anim.ResetTrigger("slash");
            
        }
    }
    public void Attack1()
    {
        // check for enemies in range in the layer assigned
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            if(enemy.tag == "Enemy")
            {
                enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
            }
            else if(enemy.tag == "Onibi")
            {
                enemy.GetComponent<OnibiScript>().TakeDamage(attackDamage);
            }
            else if(enemy.tag == "Tanuki")
            {
                enemy.GetComponent<TanukiScript>().TakeDamage(attackDamage);
            }
            
        }

    }

    public void Object(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextAttackTime && BombSelected)
        {
            // animation trigger
            anim.SetTrigger("Bomb");
            nextAttackTime = Time.time + 1f / attackRate;
        }

        if (context.performed && Time.time >= nextAttackTime && CandleSelected)
        {
            // animation trigger
            anim.SetTrigger("Candle");
            nextAttackTime = Time.time + 1f / attackRate;
        }


        if (context.canceled)
        {
            anim.ResetTrigger("Bomb");
            anim.ResetTrigger("Candle");
        }
    }

    // Code BombWall

    // Code CandleLight




    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            TakeDamage(1);
        }
        if (collision.gameObject.CompareTag("Juice"))
        {
            TakeDamageJ(2);
        }
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);

        // Hurt animation
        anim.SetTrigger("Hurt");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    public void TakeDamageJ(int damage)
    {
        CurrentHealth -= damage;
        rb.AddForce((Vector2.up * (DamageForce*3)) + (Vector2.right * (DamageForce*3)), ForceMode2D.Impulse);

        // Hurt animation
        anim.SetTrigger("Hurt");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }



    void Die()
    {
        Debug.Log("You died!");

        // Die animation
        anim.SetBool("IsDead", true);

        // Disable the player
        EnableCombat(false);

    }

    public void EnableCombat(bool boolean)
    {
        if (boolean)
        {
            playerInputActions.Player.Enable();
        }
        else
        {
            playerInputActions.Player.Disable();
        }
    }

}
