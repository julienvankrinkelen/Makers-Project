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
            case "Candle":
                playerCollectibles.pickedCandle();
                break;
            case "Scroll":
                playerCollectibles.pickScroll(collectibleObject);
                break;
            case "Omamori":
                playerCollectibles.pickOmamori(collectibleObject);
                  break;
              case "Daruma":
                playerCollectibles.pickDaruma(collectibleObject);
                  break;
          }
        print("Daruma : " + playerCollectibles.getDarumaNumber());
        print("Omamori : " + playerCollectibles.getOmamoriNumber());
        print("Dash : " + playerCollectibles.checkHasDash());
        print("Candle : " + playerCollectibles.checkHasCandle());
        print("Scroll : " + playerCollectibles.getExplosiveScrollNumber());

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




