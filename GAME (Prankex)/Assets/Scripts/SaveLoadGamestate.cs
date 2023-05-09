using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SaveLoadGamestate : MonoBehaviour
{
    [Header("Player")]
    public PlayerCombat player;
    public Transform transformPlayer;

    [Header("Ennemies")]
    public Transform transformEnnemy1;
    public float ennemy1Health;
    
    [Header("Collectibles")]
    public PlayerCollectibles playerCollectibles;
    public GameObject omamori1;
    public bool omamori1Picked;

    public bool daruma1Picked;
    public bool artifact1Picked;
    public bool scroll1Picked;

    public GameObject candleItem;
    public bool candlePicked;

    public GameObject dashItem;
    public bool dashPicked; 

  
    public int JustLoadedScene;
    public int JustDeletedSave;

    private Gamestate gamestate;


    public void Update()
    {
        // Pour dialoguer avec le script d'une autre scène, on choisir de rentrer un flag dans les PlayerPrefs quand le joueur
        // veut load une partie déjà saved. Assez lourd, mais marche pour l'instant.

        JustLoadedScene = PlayerPrefs.GetInt("JustLoadedScene");
       //print("JUST LOADED SCENE : " + JustLoadedScene);
        if (JustLoadedScene==1)
        {
            JustLoadedScene = 0;
            PlayerPrefs.SetInt("JustLoadedScene", JustLoadedScene);
            LoadGamestate();
        }

        JustDeletedSave = PlayerPrefs.GetInt("JustDeleteSave");
        //print("JustDeleted Save : " + JustDeletedSave);
        if (JustDeletedSave == 1)
        {  
            Debug.Log("DISABLE SAVE");
            DisableSave(gamestate);
        }  
    }


    public void SaveGamestate()
    {
       
        PlayerPrefs.SetInt("Save Exists", 1);
        SaveSystem.SavePlayer(this);
        
    }

    public void LoadGamestate()
    {
        JustLoadedScene = 0;
        PlayerPrefs.SetInt("JustLoadedScene", JustLoadedScene);
        int saveExists = PlayerPrefs.GetInt("Save Exists");
        if (saveExists == 1)
        {
            Gamestate data = SaveSystem.LoadGamestate();

            //player
            player.CurrentHealth = data.health;
            player.attackDamage = data.attackDamage;

            Vector2 positionPlayer;
            positionPlayer.x = data.positionPlayer[0];
            positionPlayer.y = data.positionPlayer[1];
            transformPlayer.position = positionPlayer;

            //ennemies
            Vector2 positionEnnemy1;
            positionEnnemy1.x = data.positionEnnemy1[0];
            positionEnnemy1.y = data.positionEnnemy1[1];
            transformEnnemy1.position = positionEnnemy1;

            //collectibles
            dashPicked = data.dashPicked;
            if (dashPicked)
            {
                dashItem.SetActive(false);
                playerCollectibles.setDash(true);
            }

            candlePicked = data.candlePicked;
            if (candlePicked)
            {
                candleItem.SetActive(false);
                playerCollectibles.setCandle(true);
            }

        }
        else
        {
            Debug.Log("AUCUNE SAVE DISPONIBLE");
        }
    }

    public Gamestate createGamestate(Gamestate gamestate)
    {

        this.gamestate = gamestate;
        //player
        gamestate.health = player.CurrentHealth;
        gamestate.attackDamage = player.attackDamage;

        gamestate.positionPlayer = new float[2];
        gamestate.positionPlayer[0] = transformPlayer.position.x;
        gamestate.positionPlayer[1] = transformPlayer.position.y;


        //ennemies
        gamestate.positionEnnemy1 = new float[2];
        gamestate.positionEnnemy1[0] = transformEnnemy1.position.x;
        gamestate.positionEnnemy1[1] = transformEnnemy1.position.y;


        //collectibles
        gamestate.dashPicked = playerCollectibles.checkHasDash();
        gamestate.candlePicked = playerCollectibles.checkHasCandle();

        PlayerPrefs.SetInt("JustDeleteSave", 0);
        return gamestate;
    }

    public void DisableSave(Gamestate gamestate)
    {
        // On déréférence le gamestate
        gamestate = null;
        PlayerPrefs.SetInt("JustDeleteSave", 0);
        PlayerPrefs.SetInt("Save Exists", 0);
    }
}