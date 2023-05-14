using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{

    private int currentHealth;
    private int health = 1;
    //private Animator anim;

    private void Start()
    {
        currentHealth = health;
       // anim = GetComponent<Animator>();
    }
    public void DestroyWall()
    {
        currentHealth -= 1;
        Debug.Log("Wall destroyed ");

        StartCoroutine(Explosion());

    }

    public void DestroyBush()
    {
        currentHealth -= 1;
        Debug.Log("Bush destroyed ");

        StartCoroutine(Burning());
    }

    private IEnumerator Explosion()
    {
        // Destroyed animation
        //anim.SetTrigger("Destroyed");
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    private IEnumerator Burning()
    {
        //anim.SetTrigger("Burning");
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
