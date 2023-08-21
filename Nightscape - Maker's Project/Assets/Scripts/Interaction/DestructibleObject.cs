using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public TerrainState terrainState;



    //private Animator anim;

    private void Start()
    {
       // anim = GetComponent<Animator>();
    }
    public void DestroyWall()
    {
        Debug.Log("Wall destroyed ");
        terrainState.destroyWall(gameObject);
        StartCoroutine(Explosion());

    }

    public void DestroyBush()
    {
        Debug.Log("Bush destroyed ");
        terrainState.destroyBush(gameObject);
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
