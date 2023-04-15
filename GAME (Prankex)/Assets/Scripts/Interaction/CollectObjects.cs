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
    private enum Collectible { Omamori, Daruma, Dash, Candle, Artefact }
    void Start()
    {
    
        print(playerCollectibles);
        collectible = gameObject.tag;
        collectibleObject.SetActive(true);
        
    }
   

    public void CollectObject()
    {
        
        Debug.Log("COLLECT OBJECT : " + collectible);      
          switch(collectible)
          {
              case "Dash":
                playerCollectibles.pickedDash(); 
                  break;
            case "Artefact":
                playerCollectibles.addArtefact();
                break;
            case "Candle":
                playerCollectibles.pickedCandle();
                break;

            case "Omamori":   
                playerCollectibles.addOmamori();
                  break;
              case "Daruma":
                playerCollectibles.addDaruma();
                  break;
          }
        print("Daruma : " + playerCollectibles.getDarumaNumber());
        print("Omamori : " + playerCollectibles.getOmamoriNumber());
        print("Artefact : " + playerCollectibles.getArtefactNumber());
        print("Dash : " + playerCollectibles.checkHasDash());
        print("Candle : " + playerCollectibles.checkHasCandle());
    

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




