using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private Collider2D playercollider;
    private float attackDamage = 1;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D == playercollider)
        {
            playercollider.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        }
    }


}
