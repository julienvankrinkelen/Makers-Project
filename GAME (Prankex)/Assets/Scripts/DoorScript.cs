using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    private Animator anim;
    private EdgeCollider2D coll;
    public BoxCollider2D playercollider;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<EdgeCollider2D>();
        anim.SetBool("FirstCandle", false);
        anim.SetBool("TwoCandle", false);
        anim.SetBool("ThreeCandle", false);
    }

    // Update is called once per frame
    void Update()
    {
       if(anim.GetBool("ThreeCandle") == true)
        {
            Physics2D.IgnoreCollision(coll, playercollider);
        }
    }
}
