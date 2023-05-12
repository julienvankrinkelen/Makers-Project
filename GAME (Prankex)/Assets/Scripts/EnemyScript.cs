using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    

    public float maxHealth = 2f;
    public float currentHealth;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform playerTransform;
    public PlayerCombat playerCombat;

    public float attackRange = 0.5f;
    public float attackDamage = 2f;
    public float attackRate = 4f;
    public float nextAttackTime = 0f;

    private bool isDead = false;

    public GameObject coin;
    public AIPath aiPath;

    void Start()
    {
        currentHealth = maxHealth;
        coin.SetActive(false);

    }

    private void Update()
    {
        if (Mathf.Abs(playerTransform.position.x - attackPoint.position.x) < 10 && Mathf.Abs(playerTransform.position.y - attackPoint.position.y) < 3 && playerCombat.CurrentHealth>0)
        {
            anim.SetBool("CombatMode", true);
            
        }
        else
        {
            anim.SetBool("CombatMode", false);
        }
        

        if(anim.GetBool("CombatMode") == true && anim.GetBool("IsDead") == false && playerCombat.CurrentHealth>0)
        {
            GetComponent<EnemyAI>().followEnabled = true;
        }
        else
        {
            GetComponent<EnemyAI>().followEnabled = false;
        }

        
        if((Vector2.Distance(playerTransform.position, attackPoint.position) < 1 ) && Time.time >= nextAttackTime && playerCombat.CurrentHealth>0)
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


    public void TakeDamage(float damage)
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

        // GetComponent<Collider2D>().enabled = false;
        Physics2D.IgnoreLayerCollision(3, 8);
        GetComponent<EnemyAI>().enabled = false;
        coin.SetActive(true);
        




    }

    
}
