using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectibles : MonoBehaviour
{
    private int coinNumber;
    private int dollNumber;
    private int charmNumber;


    public void Start()
    {
     coinNumber=0;
     dollNumber=0;
     charmNumber=0;


    }
    public void addCoin()
    {
        coinNumber++;
    }
    public void addDoll()
    {
        dollNumber++;
    }
    public void addCharm()
    {
        charmNumber++;
    }
    public int getCoinNumber()
    {
        return coinNumber;
    }
    public int getDollNumber()
    {
        return dollNumber;
    }
    public int getCharmNumber()
    {
        return charmNumber;
    }
}
