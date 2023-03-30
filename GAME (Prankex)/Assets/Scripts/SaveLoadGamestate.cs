using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SaveLoadGamestate : MonoBehaviour
{
    //player
    public PlayerCombat player;
    public Transform transformPlayer;

    //ennemies
    public Transform transformEnnemy1;

    public int JustLoadedScene;
    public int JustDeletedSave;

    private Gamestate gamestate;


    public void Update()
    {
        // Pour dialoguer avec le script d'une autre scène, on choisir de rentrer un flag dans les PlayerPrefs quand le joueur
        // veut load une partie déjà saved. Assez lourd, mais marche pour l'instant.

        JustLoadedScene = PlayerPrefs.GetInt("JustLoadedScene");
        if (JustLoadedScene==1)
        {
            LoadGamestate();
        }

        JustDeletedSave = PlayerPrefs.GetInt("JustDeleteSave");
        //print("JustDeleted Save : " + JustDeletedSave);
        if (JustDeletedSave == 1 && gamestate!=null)
        {  
            Debug.Log("DISABLE SAVE");
            DisableSave(gamestate);
        }  
    }


    public void SaveGamestate()
    {
        int saveExists = 1;
        PlayerPrefs.SetInt("Save Exists", saveExists);
        SaveSystem.SavePlayer(this);
        
    }

    public void LoadGamestate()
    {
        if (gamestate != null && gamestate.enableSave)
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
  

        }
        else
        {
            Debug.Log("AUCUNE SAVE DISPONIBLE");
        }
        JustLoadedScene = 0;
        PlayerPrefs.SetInt("JustLoadedScene", JustLoadedScene);
    }

    public Gamestate createGamestate(Gamestate gamestate)
    {

        this.gamestate = gamestate;
        gamestate.enableSave = true;

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
        return gamestate;
    }

    public void DisableSave(Gamestate gamestate)
    {
        gamestate.enableSave = false;
        PlayerPrefs.SetInt("JustDeleteSave", 0);
        PlayerPrefs.SetInt("Save Exists", 0);
    }
}