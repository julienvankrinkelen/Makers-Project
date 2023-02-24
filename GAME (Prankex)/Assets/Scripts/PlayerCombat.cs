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
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public int PlayerHealth = 100;
    int CurrentHealth;

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
    // Update is called once per frame
    void Update()
    {
        // check la hitbox
        
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
            enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
        }

    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        PlayerHealth -= damage;

        // Hurt animation
        anim.SetTrigger("Hurt");

        if (PlayerHealth < 0)
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
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

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
