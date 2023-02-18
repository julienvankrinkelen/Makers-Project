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


    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        

        

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
            // check for enemies in range in the layer assigned
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            // animation trigger
            anim.SetTrigger("slash");


            foreach(Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit " + enemy.name);
                enemy.GetComponent<EnemyScript>().TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + 1f / attackRate;

        }

        if (context.canceled)
        {
            anim.ResetTrigger("slash");
            
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
