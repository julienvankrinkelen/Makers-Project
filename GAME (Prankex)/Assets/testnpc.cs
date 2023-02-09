using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class testnpc : MonoBehaviour
{
    SpriteRenderer sr;
    void Start() {

         sr = gameObject.GetComponent<SpriteRenderer>();

        }
    public void testDialog()
    {
        Debug.Log("Pressed F");
        if(sr.flipX == true) { sr.flipX= false; }
        else if (sr.flipX == false) { sr.flipX = truezdq; }
    }
}
