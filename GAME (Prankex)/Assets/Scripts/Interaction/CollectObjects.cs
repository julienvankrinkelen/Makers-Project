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



    public PlayerCollectibles playerCollectibles;

    private string collectible;
    //Link to parent object of "Collectible circle". This parent contains the sprite etc.
    [SerializeField] private GameObject collectibleObject;
    private enum Collectible { Coin, Doll, Charm }
    void Start()
    {
        /*if (!playerCollectibles.getCharmNumber())
        {
            playerCollectibles.PlayerCollectibles(
        }*/
        
        print(playerCollectibles);
        collectible = gameObject.tag;
        collectibleObject.SetActive(true);
        
    }
   

    public void CollectObject()
    {
        
        Debug.Log("COLLECT OBJECT : " + collectible);      
          switch(collectible)
          {
              case "Flute":
                playerCollectibles.pickedFlute(); 
                  break;
              case "Doll":   
                playerCollectibles.addDaruma();
                  break;
              case "Charm":
                playerCollectibles.addOmamori();
                  break;
          }
        print("Daruma : " + playerCollectibles.getDarumaNumber());
        print("Omamori : " + playerCollectibles.getOmamoriNumber());

        collectibleObject.SetActive(false);
    }
        
   




    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectObject();
           

        }
    }
 
}




