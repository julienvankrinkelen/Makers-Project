using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnibiAttack : MonoBehaviour
{
    public Collider2D playercollider;


    public float attackDamage = 1;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        playercollider.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
    }


}
