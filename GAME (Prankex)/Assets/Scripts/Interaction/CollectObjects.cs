using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

public class CollectObjects : MonoBehaviour
{
  



    private string collectible;

    private int coinNumber;
    private int dollNumber;
    private int charmNumber;

    private enum Collectible { Coin, Doll, Charm }
    void Start()
    {
        collectible = gameObject.tag;
        gameObject.SetActive(true);
    }
   

    public void CollectObject()
    {
        Debug.Log("COLLECT OBJECT : " + collectible);      
          switch(collectible)
          {
              case "Coin":
                  coinNumber++; 
                  break;
              case "Doll":
                  dollNumber++;
                  break;
              case "Charm":
                  charmNumber++;
                  break;
          }
        //print(coinNumber);
        gameObject.SetActive(false);
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

