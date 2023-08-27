using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private TerrainState terrainState;

    public void DestroyWall()
    {
       // Debug.Log("Wall destroyed ");
        terrainState.destroyWall(gameObject);
        StartCoroutine(Explosion());
    }

    public void DestroyBush()
    {
      //  Debug.Log("Bush destroyed ");
        terrainState.destroyBush(gameObject);
        StartCoroutine(Burning());
    }

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    private IEnumerator Burning()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
