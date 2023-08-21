using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Collider2D playercollider;


    public float attackDamage = 1;


    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == playercollider)
        {
            playercollider.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
        
    }

    
}
