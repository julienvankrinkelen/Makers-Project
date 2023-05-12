using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{

    public Transform attackPoint;

    private float currentHealth = 1;
    private float Health;

    private void Awake()
    {
        Health = currentHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Box destroyed ");
        
        // Destroyed animation
        // anim.SetTrigger("Destroyed");
        gameObject.SetActive(false);

    }
}
