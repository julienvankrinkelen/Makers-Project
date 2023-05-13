using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerCombat : MonoBehaviour
{
    
    private PlayerInputActions playerInputActions;
    public Transform PlayerTransform;
    public SaveLoadGamestate saveLoadGamestate;
    public PlayerMovement playerMovement;

    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public Animator anim;
    public GameObject panelTransiDeath;

    [SerializeField] private LayerMask jumpableGround;
    public LayerMask enemyLayers;
    public Transform attackPoint;

    public float attackRange = 0.5f;
    public float attackDamage = 1f;
    public float attackRate = 2f;
    public float nextAttackTime = 0f;

    public bool ScrollSelected = false;
    public bool CandleSelected = false;

    public float PlayerHealth = 4f;
    public float CurrentHealth;
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
    /*
    public void Object(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextAttackTime && ScrollSelected)
        {
            // animation trigger
            anim.SetTrigger("Scroll");
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
            anim.ResetTrigger("Scroll");
            anim.ResetTrigger("Candle");
        }
    }

    // Code DestroyWall

    // Code CandleLight

    */


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
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);

        // Hurt animation
        anim.SetTrigger("Hurt");

        if (CurrentHealth <= 0)
        {
            StartCoroutine(Die());
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



    public IEnumerator Die()
    {
        Debug.Log("You died!");

        // Die animation
        anim.SetBool("IsDead", true);
        // Disable the player and its extern interactions
        EnableCombat(false);
        playerMovement.EnableMovement(false);

        yield return new WaitForSeconds(1);
        anim.SetBool("IsDead", false);
        //ECRAN NOIR TRANSI
        panelTransiDeath.SetActive(true);
        yield return new WaitForSeconds(1);
        // DESACTIVER ECRAN NOIR
        panelTransiDeath.SetActive(false);
        playerMovement.EnableMovement(true);
        EnableCombat(true);
        // Load derni�re save 
        // Si save non existante : recommencer une new game
        int saveExists = PlayerPrefs.GetInt("Save Exists");
        if(saveExists == 1)
        {
            saveLoadGamestate.LoadGamestate();
        }
        else
        {
            SceneManager.LoadScene("new map");
        }

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

    //Daruma bonus atk permanent
    public void AddDamage(float damage)
    {
        attackDamage += damage;
    }

    //Omamori bonus hp permanent
    public void AddLife(float life)
    {
        CurrentHealth += life;
    }

}
