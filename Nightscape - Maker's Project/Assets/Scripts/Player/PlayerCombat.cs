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
    // public GameObject Boss;
    public GameObject Bosscopy;
    public PauseMenu pauseMenu;

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
    public float attackRate = 1f;
    public float nextAttackTime = 0f;

    [SerializeField] private AudioSource hurtSoundEffect;
    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;
    [SerializeField] private AudioSource itemUsedSoundEffect;
    [SerializeField] private AudioSource interactClickSoundEffect;
    [SerializeField] private AudioSource healStatueSoundEffect;
    [SerializeField] private AudioSource openStatueSoundEffect;
    [SerializeField] private AudioSource wallDestroySoundEffect;
    [SerializeField] private AudioSource bushDestroySoundEffect;
    [SerializeField] private AudioSource lanternLightenSoundEffect;
    [SerializeField] private AudioSource levelUpSoundEffect;

    private float takeDamageCooldown;
    private float takeDamageFixedTime = 0.5f;

    public bool ScrollSelected = false;
    public bool CandleSelected = false;

    public float maxHealth = 4f;
    public float CurrentHealth;
    private bool IsDead = false;
    public bool InBossCombat = false;

    [SerializeField] private float DamageForce = 13;
    private void Start()
    {
        deathAnim = deathAnimObject.GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        CurrentHealth = maxHealth;
        panelTransiDeath.SetActive(false);
    }
    private void Awake()
    {

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Slash.performed += Slash;

    }

    private void Update()
    {
        takeDamageCooldown -= Time.deltaTime;
    }

    public void Slash(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextAttackTime)
        {
            // animation trigger
            anim.SetTrigger("slash");
            attackSoundEffect.Play();
            nextAttackTime = Time.time + 1f / attackRate;

        }

        if (context.canceled)
        {
            anim.ResetTrigger("slash");
            
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Tanuki"))
        {
            collider2D.GetComponent<TanukiScript>().TakeDamage(attackDamage);
        }
        else if(collider2D.CompareTag("Onibi"))
        {
            collider2D.GetComponent<OnibiScript>().TakeDamage(attackDamage);
        }
        else if(collider2D.CompareTag("Enemy"))
        {
            collider2D.GetComponent<EnemyScript>().TakeDamage(attackDamage);
        }
        else if (collider2D.CompareTag("Boss"))
        {
            collider2D.GetComponent<BossScript>().TakeDamage(attackDamage);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps") && CurrentHealth>0)
        {
            TakeDamageJ(1,2);
        }
        if (collision.gameObject.CompareTag("Juice") && CurrentHealth>0)
        {
            TakeDamageJ(2, 3);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps") && CurrentHealth > 0)
        {
            TakeDamageJ(1, 2);
        }
        if (collision.gameObject.CompareTag("Juice") && CurrentHealth > 0)
        {
            TakeDamageJ(2, 3);
        }
    }
    public void TakeDamage(float damage)
    {
        if (CurrentHealth>0 && takeDamageCooldown < 0)
        {

            CurrentHealth -= damage;
            rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);

            // Hurt animation
            anim.SetTrigger("Hurt");
            hurtSoundEffect.Play();
            takeDamageCooldown = takeDamageFixedTime;
        }
        if (CurrentHealth <= 0 && !IsDead)
        {
            StartCoroutine(Die());
        }
    }
    public void TakeDamageJ(int damage, int knockback)
    {
        if (CurrentHealth > 0 && takeDamageCooldown < 0)
        {
            CurrentHealth -= damage;
            rb.AddForce((Vector2.up * (DamageForce * knockback)) + (Vector2.right * (DamageForce * knockback)), ForceMode2D.Impulse);

            // Hurt animation
            anim.SetTrigger("Hurt");
            hurtSoundEffect.Play();
            takeDamageCooldown = takeDamageFixedTime;
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

        // Loading Screen
        panelTransiDeath.SetActive(true);
        deathAnim.SetBool("ShowLoadingScreen", true);
        // Die animation
        anim.SetBool("IsDead", true);
        dieSoundEffect.Play();
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
            if (InBossCombat)
            {
                GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss");

                Debug.Log("Boss length =  " + boss.Length);
                for (int i = 0; i < boss.Length; i++)
                {
                    if (boss[i].activeSelf == true)
                    {
                        boss[i].GetComponent<BossScript>().Autodestruction();
                    }
                }
                GameObject bossclone = Instantiate(Bosscopy);
                bossclone.SetActive(true);
                // bossScript.ResetBoss();

                InBossCombat = false;
                
            }
        }
        else
        {
            Debug.Log("Loading new game bc no save before dying");
            SceneManager.LoadScene("new map");
        }

        pauseMenu.EnablePauseEchap(true);


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
        healStatueSoundEffect.Play();
        float toHeal = maxHealth - CurrentHealth;
        CurrentHealth+= toHeal;
        Debug.Log("Just healed player for " + toHeal + " HP");
    }

    public void SoundItemUsed()
    {
        itemUsedSoundEffect.Play();
    }
    public void InteractClickItemUsed()
    {
        interactClickSoundEffect.Play();
    }

    public void SoundWallDestroy()
    {
        StartCoroutine(WallSoundDelay());
    }

    private IEnumerator WallSoundDelay()
    {
        yield return new WaitForSecondsRealtime(3);
        wallDestroySoundEffect.Play();
    }
    public void SoundBushDestroy()
    {
        StartCoroutine(BushSoundDelay());
    }
    private IEnumerator BushSoundDelay()
    {
        yield return new WaitForSecondsRealtime(3);
        bushDestroySoundEffect.Play();
    }
    public void SoundLanternLighten()
    {
        lanternLightenSoundEffect.Play();
    }

    public void SoundLevelUp()
    {
        levelUpSoundEffect.Play();
    }

    public void SoundOpenStatue()
    {
        openStatueSoundEffect.Play();
    }


}
