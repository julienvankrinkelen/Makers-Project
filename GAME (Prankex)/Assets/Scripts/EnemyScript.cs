using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    

    public int maxHealth = 200;
    int currentHealth;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform playerTransform;

    public float attackRange = 0.5f;
    public int attackDamage = 20;
    public float attackRate = 2f;
    float nextAttackTime = 0f;


    public AIPath aiPath;

    void Start()
    {
        currentHealth = maxHealth;

    }

    private void Update()
    {
        if (currentHealth < maxHealth)
        {
            anim.SetBool("CombatMode", true);
            
        }
        if(anim.GetBool("CombatMode") == true && anim.GetBool("IsDead") == false)
        {
            aiPath.enabled = true;
        }
        if(aiPath.desiredVelocity.x >= 0.01f)
        {
            anim.SetBool("Run", true);
            sprite.flipX = true;
            
        }
        if(aiPath.desiredVelocity.x <= -0.01f)
        {
            sprite.flipX = false;
            anim.SetBool("Run", true);
            
        }
        if (sprite.flipX == true)
        {
            Debug.Log(sprite.flipX + " Its flipped");
            Vector3 position = attackPoint.position;
            position.x = 0.69f;
        }
        else
        {
            Vector3 position = attackPoint.position;
            position.x = 0.69f;
        }

        if (aiPath.desiredVelocity.x == 0)
        {
            
            anim.SetBool("Run", false);
        }
        
        if((Vector2.Distance(playerTransform.position, attackPoint.position) < 1 ) && Time.time >= nextAttackTime)
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
        currentHealth -= damage;

        // Hurt animation
        anim.SetTrigger("Hurt");

        if (currentHealth < 0)
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
        
        GetComponent<Collider2D>().enabled = false;
        aiPath.enabled = false;


    }
    
}
