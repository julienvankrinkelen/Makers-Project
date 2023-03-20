using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{

    public Transform attackPoint;

    private int currentHealth = 1;
    private int Health;

    private void Awake()
    {
        Health = currentHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Box destroyed ");
        
        // Destroyed animation
        // anim.SetTrigger("Destroyed");
        gameObject.SetActive(false);

    }
}
