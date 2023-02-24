using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LayerMask playerLayer;
    public Transform attackPoint;
    public Transform playerTransform;

    public float attackRange = 0.5f;
    public int attackDamage = 20;



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

}
