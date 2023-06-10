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

    public GameObject deathAnimObject;
    public Animator deathAnim;

    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public Animator anim;
    public GameObject panelTransiDeath;

    [SerializeField] private LayerMask jumpableGround;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public float attackDamage = 1f;
    public float attackRate = 2f;
    public float nextAttackTime = 0f;
    private EdgeCollider2D airattackcoll;
    private CapsuleCollider2D attackcoll;

    public bool ScrollSelected = false;
    public bool CandleSelected = false;

    public float maxHealth = 4f;
    public float CurrentHealth;
    private bool IsDead = false;
    [SerializeField] private float DamageForce = 13;
    private void Start()
    {
        deathAnim = deathAnimObject.GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        airattackcoll = GetComponent<EdgeCollider2D>();
        attackcoll = GetComponent<CapsuleCollider2D>();

        CurrentHealth = maxHealth;
        panelTransiDeath.SetActive(false);
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

    public void OnTriggerEnter2D(Collider2D Collider2D)
    {
        if (Collider2D.CompareTag("Tanuki"))
        {
            Collider2D.GetComponent<TanukiScript>().TakeDamage(attackDamage);
        }
        else if(Collider2D.CompareTag("Onibi"))
        {
            Collider2D.GetComponent<OnibiScript>().TakeDamage(attackDamage);
        }
        else if(Collider2D.CompareTag("Enemy"))
        {
            Collider2D.GetComponent<EnemyScript>().TakeDamage(attackDamage);
        }
        else if (Collider2D.CompareTag("Boss"))
        {
            Collider2D.GetComponent<BossScript>().TakeDamage(attackDamage);
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
        if (!IsDead)
        {

            CurrentHealth -= damage;
            rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);

            // Hurt animation
            anim.SetTrigger("Hurt");
        }
        if (CurrentHealth <= 0 && !IsDead)
        {
            StartCoroutine(Die());
        }
    }
    public void TakeDamageJ(int damage)
    {
        if (!IsDead)
        {
            CurrentHealth -= damage;
            rb.AddForce((Vector2.up * (DamageForce * 3)) + (Vector2.right * (DamageForce * 3)), ForceMode2D.Impulse);

            // Hurt animation
            anim.SetTrigger("Hurt");
        }
        if (CurrentHealth <= 0 && !IsDead)
        {
            StartCoroutine(Die());
        }
    }



    public IEnumerator Die()
    {
        IsDead = true;
        Debug.Log("You died!");
        
        // Disable the player and its extern interactions

        EnableCombat(false);
        playerMovement.EnableMovement(false);

        // Loading Screenm
        panelTransiDeath.SetActive(true);
        deathAnim.SetBool("ShowLoadingScreen", true);
        // Die animation
        anim.SetBool("IsDead", true);
        yield return new WaitForSeconds(1);

        //GetComponent<PlayerMovement>().enabled = false;
        this.sprite.enabled = false;
       

        IsDead = false;

        yield return new WaitForSeconds(5);
        anim.SetBool("IsDead", false);

        playerMovement.EnableMovement(true);
        EnableCombat(true);
        Debug.Log("Enabling movement & combat after death !");
        this.sprite.enabled = true;


        // Load derni�re save 
        // Si save non existante : recommencer une new game
        int saveExists = PlayerPrefs.GetInt("Save Exists");
        if(saveExists == 1)
        {
            Debug.Log("Loading previous save after dying");
            saveLoadGamestate.LoadGamestate();
        }
        else
        {
            Debug.Log("Loading new game bc no save before dying");
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
        maxHealth += life;

    }
    // Heal aux autels : remet le player full life en fonction de ses HP max
    public void Heal()
    {
        float toHeal = maxHealth - CurrentHealth;
        CurrentHealth+= toHeal;
        Debug.Log("Just healed player for " + toHeal + " HP");
    }

}
