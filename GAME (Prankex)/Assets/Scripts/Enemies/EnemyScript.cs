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
    

    public float maxHealth = 2f;
    public float currentHealth;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform playerTransform;
    public PlayerCombat playerCombat;

    public float attackRange = 0.5f;
    public int attackDamage = 2;
    public float attackRate = 0.5f;
    float nextAttackTime = 0f;
    [SerializeField] private float DamageForce = 13;

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
        if (Vector2.Distance(playerTransform.position, attackPoint.position) < 10 && anim.GetBool("IsDead") == false)
        {
            GetComponent<EnemyAI>().followEnabled = true;

        }
        else if(Vector2.Distance(playerTransform.position, attackPoint.position) > 50 && anim.GetBool("IsDead") == false)
        {
            GetComponent<EnemyAI>().followEnabled = false;
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


    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            Debug.Log("Enemy health : " + currentHealth);
            // Hurt animation
            anim.SetTrigger("Hurt");
            rb.AddForce((Vector2.up * DamageForce) + (Vector2.right * DamageForce), ForceMode2D.Impulse);
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
        StartCoroutine(Bunshin());

    }

    private IEnumerator Bunshin()
    {
        GetComponent<EnemyScript>().enabled = false;
        // anim.SetBool("IsDead", true);
        // Die animation
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        //coin.SetActive(true);
    }

}
