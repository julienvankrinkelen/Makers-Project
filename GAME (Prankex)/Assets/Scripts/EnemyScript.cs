using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;

    public int maxHealth = 200;
    int currentHealth;

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
        
        
    }
    
}
