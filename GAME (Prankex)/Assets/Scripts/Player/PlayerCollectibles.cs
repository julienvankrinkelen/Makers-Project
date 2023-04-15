using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectibles : MonoBehaviour
{
    private int darumaNumber;
    private int omamoriNumber;
    private bool flute;
    
   


    public void Start()
    {
     darumaNumber =0;
     omamoriNumber =0;
     flute = false;


    }

    public void pickedFlute()
    {
        flute = true;
    }
    public void addDaruma()
    {
        darumaNumber++;
    }
    public void addOmamori()
    {
        omamoriNumber++;
    }



  
    public int getDarumaNumber()
    {
        return darumaNumber;
    }
    public int getOmamoriNumber()
    {
        return omamoriNumber;
    }
}
