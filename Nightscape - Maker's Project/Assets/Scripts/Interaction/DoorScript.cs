using Pathfinding;
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


    public void setCandleAnim(int numberOfCandles)
    {
        //Uniquement possiblement appelée par SaveLoadGamestate
        if(numberOfCandles == 0) 
        {
            Debug.Log("Aucune lanterne n'a été allumée selon la dernière save");
        }
        else if(numberOfCandles == 1) 
        {
            anim.SetBool("FirstCandle", true);
        }
        else if(numberOfCandles == 2)
        {
            anim.SetBool("TwoCandle", true);
        }
        else if(numberOfCandles == 3)
        {   
            //Three lanterns were lightened, door is unlocked : player can go through
            anim.SetBool("ThreeCandle", true) ;
            Physics2D.IgnoreCollision(coll, playercollider);
        }
    }
}
