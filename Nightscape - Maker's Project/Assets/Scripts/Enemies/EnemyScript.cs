using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private MobsState mobsState;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D playercollider;
    

    public float maxHealth = 3f;
    public float currentHealth;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerCombat playerCombat;

    [SerializeField] private AudioSource dieSoundEffect;
    [SerializeField] private AudioSource hurtSoundEffect;
    [SerializeField] private AudioSource attackSoundEffect;

    private float attackRange = 0.5f;
    private int attackDamage = 2;
    private float attackRate = 0.5f;
    private float nextAttackTime = 0f;
    [SerializeField] private float DamageForce = 13;

    private bool isDead = false;

    [SerializeField] private AIPath aiPath;

    void Start()
    {
        currentHealth = maxHealth;

    }

    private void Update()
    {
        if (Vector2.Distance(playerTransform.position, attackPoint.position) < 10 && anim.GetBool("IsDead") == false && playerCombat.CurrentHealth>0)
        {
            GetComponent<EnemyAI>().followEnabled = true;
            anim.SetBool("CombatMode", true);

        }
        else if(Vector2.Distance(playerTransform.position, attackPoint.position) > 50 && anim.GetBool("IsDead") == false)
        {
            GetComponent<EnemyAI>().followEnabled = false;
            anim.SetBool("CombatMode", false);
        }

        
        if((Vector2.Distance(playerTransform.position, attackPoint.position) < 2 ) && Time.time >= nextAttackTime && playerCombat.CurrentHealth>0)
        {
            anim.SetTrigger("Attack");
            attackSoundEffect.Play();
            nextAttackTime = Time.time + 3f / attackRate;
        }

    }
    public void Attack1()
    {
        // check for player in range in the layer assigned
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("You have been hit");
            enemy.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }

    }


    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            Debug.Log("Enemy health was : " + currentHealth);
            currentHealth -= damage;
            Debug.Log("Enemy health now : " + currentHealth);
            // Hurt animation
            anim.SetTrigger("Hurt");
            hurtSoundEffect.Play();
            rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);
            //Registering karakasa life into mobs state save script
            mobsState.registerLifeKarakasa(gameObject, this.currentHealth);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Traps"))
        {
            TakeDamage(1);
        }
        if (collision.gameObject.CompareTag("Juice"))
        {
            TakeDamage(2);
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Die animation
        anim.SetBool("IsDead", true);
        dieSoundEffect.Play();

        // Disable the enemy
        isDead = true;

        GetComponent<Collider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), playercollider);
        GetComponent<EnemyAI>().enabled = false;
        StartCoroutine(Bunshin());

    }

    private IEnumerator Bunshin()
    {
        GetComponent<EnemyScript>().enabled = false;
        // Die animation
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

}