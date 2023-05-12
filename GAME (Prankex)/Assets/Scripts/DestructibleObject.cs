using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{

    private int currentHealth = 1;
    private int Health;

    private void Start()
    {
        Health = currentHealth;
    }
    public void BoomWall(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Wall destroyed ");

        StartCoroutine(Explosion());

    }

    private IEnumerator Explosion()
    {
        // Destroyed animation
        // anim.SetTrigger("Destroyed");
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
