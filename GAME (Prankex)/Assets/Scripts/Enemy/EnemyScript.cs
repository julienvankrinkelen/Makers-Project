using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public Collider2D playercollider;
    

    public int maxHealth = 2;
    int currentHealth;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform playerTransform;

    public float attackRange = 0.5f;
    public int attackDamage = 2;
    public float attackRate = 4f;
    float nextAttackTime = 0f;

    private bool isFacingTheRight = false;
    private bool isDead = false;

    // public GameObject coin;

    public AIPath aiPath;

    void Start()
    {
        currentHealth = maxHealth;
        // coin.SetActive(false);

    }

    private void Update()
    {
        if (Vector2.Distance(playerTransform.position, attackPoint.position) < 10)
        {
            anim.SetBool("CombatMode", true);
            
        }

        if(anim.GetBool("CombatMode") == true && anim.GetBool("IsDead") == false)
        {
            GetComponent<EnemyAI>().followEnabled = true;
        }

        
        if((Vector2.Distance(playerTransform.position, attackPoint.position) < 2 ) && Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");
            nextAttackTime = Time.time + 1f / attackRate;
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


    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            Debug.Log("Enemy health : " + currentHealth);
            // Hurt animation
            anim.SetTrigger("Hurt");
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Die animation
        anim.SetBool("IsDead", true);

        // Disable the enemy
        isDead = true;

        GetComponent<Collider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), playercollider);
        GetComponent<EnemyAI>().enabled = false;
        // coin.SetActive(true);

    }

    
}
