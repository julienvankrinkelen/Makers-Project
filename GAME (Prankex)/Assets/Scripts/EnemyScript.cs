using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    

    public int maxHealth = 2;
    int currentHealth;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform playerTransform;

    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    private bool isFacingTheRight = false;

    public GameObject coin;


    public AIPath aiPath;

    void Start()
    {
        currentHealth = maxHealth;
        coin.SetActive(false);

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

        Flip();
        

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
        Debug.Log("Enemy health : " + currentHealth);
        // Hurt animation
        anim.SetTrigger("Hurt");

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
        
        GetComponent<Collider2D>().enabled = false;
        aiPath.enabled = false;
        coin.SetActive(true);




    }
    private void Flip()
    {
        if (aiPath.desiredVelocity.x <= -0.01f && isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            anim.SetBool("Run", true);
            isFacingTheRight = false;
        }
        else if (aiPath.desiredVelocity.x >= 0.01f && !isFacingTheRight)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            anim.SetBool("Run", true);
            isFacingTheRight = true;
        }
    }
    
}
