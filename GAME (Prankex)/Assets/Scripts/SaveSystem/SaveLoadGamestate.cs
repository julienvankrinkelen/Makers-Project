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

    public GameObject[] omamori;
    public bool[] omamoriPicked = new bool[15];

    public GameObject[] daruma;
    public bool[] darumaPicked = new bool[15];

    public GameObject[] scrolls;
    public bool[] scrollPicked = new bool[16];

    public GameObject candleItem;
    public bool candlePicked;

    public GameObject dashItem;
    public bool dashPicked;

    public GameObject[] notes;
    public bool[] notePicked = new bool[5];

    [Header("Terrain")]
    public TerrainState terrainState;

    public GameObject[] walls;
    public bool[] wallDestroyed = new bool[12];

    public GameObject[] bushes;
    public bool[] bushDestroyed = new bool[4];

    public bool[] lanternLightened = new bool[3];


    public int JustLoadedScene;
    public int JustDeletedSave;

    private Gamestate gamestate;


    public void Update()
    {
        // Pour dialoguer avec le script d'une autre sc�ne, on choisir de rentrer un flag dans les PlayerPrefs quand le joueur
        // veut load une partie d�j� saved. Assez lourd, mais marche pour l'instant.

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
        StartCoroutine(TempoLoadGamestate());

    }

    public IEnumerator TempoLoadGamestate()
    {
        //Tempo pour laisser le temps au jeu de charger tous les �l�ments, puis on les change
        yield return new WaitForSeconds(1);
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

            //dash
            dashPicked = data.dashPicked;
            if (dashPicked)
            {
                dashItem.SetActive(false);
                playerCollectibles.setDash(true);
            }

            //candle
            candlePicked = data.candlePicked;
            if (candlePicked)
            {
                candleItem.SetActive(false);
                playerCollectibles.setCandle(true);
            }

            //daruma
            darumaPicked = data.darumaPicked;
            for (int i = 0; i < darumaPicked.Length; i++)
            {   //Si le daruma a �t� pick
                if (darumaPicked[i])
                {
                    daruma[i].SetActive(false);
                    //Ajoute le daruma au compteur, dans le cas o� on aurait besoin du nombre
                    playerCollectibles.addDaruma();
                    playerCollectibles.darumaPicked[i] = true;
                }
                else
                {
                    playerCollectibles.darumaPicked[i] = false;
                }
            }

            //omamori
            omamoriPicked = data.omamoriPicked;
            for(int i=0; i<omamoriPicked.Length; i++)
            {   //Si l'omamori a �t� pick
                if (omamoriPicked[i])
                {
                    omamori[i].SetActive(false);
                    //Ajoute l'omamori au compteur, dans le cas o� on aurait besoin du nombre
                    playerCollectibles.addOmamori();
                    playerCollectibles.omamoriPicked[i] = true;
                }
                else
                {
                    playerCollectibles.omamoriPicked[i] = false;
                }
            }


            //scroll
            scrollPicked = data.scrollPicked;
            for (int i = 0; i < scrollPicked.Length; i++)
            {   //Si le scroll a �t� pick
                if (scrollPicked[i])
                {
                    scrolls[i].SetActive(false);
                    //Ajoute le scroll au compteur, dans le cas o� on aurait besoin du nombre
                    playerCollectibles.addExplosiveScroll();
                    playerCollectibles.scrollPicked[i] = true;
                }
                else
                {
                    playerCollectibles.scrollPicked[i] = false;
                }
            }

            //notes
            notePicked = data.notePicked;
            for(int i = 0; i< notePicked.Length; i++)
            {   //Si la note a �t� pick
                if (notePicked[i])
                {
                    notes[i].SetActive(false);
                    playerCollectibles.notePicked[i] = true;
                }
                else
                {
                    playerCollectibles.notePicked[i] = false;
                }
            }

            //lanterns
            lanternLightened = data.lanternLightened;
            for (int i = 0; i < lanternLightened.Length; i++)
            {   //Si la lanterne a �t� allum�e
                if (lanternLightened[i])
                {
                    playerCollectibles.setLanternLightened(i,true);
                    playerCollectibles.lanternLightened[i] = true;
                }
                else
                {
                    playerCollectibles.lanternLightened[i] = false;
                }
            }


            //walls
            wallDestroyed = data.wallDestroyed;
            for (int i = 0; i < wallDestroyed.Length; i++)
            {   //Si le wall a �t� d�truit
                if (wallDestroyed[i])
                {
                    walls[i].SetActive(false);
                    terrainState.wallDestroyed[i] = true;
                }
                else
                {
                    terrainState.wallDestroyed[i] = false;
                }
            }

            //bushes
            bushDestroyed = data.bushDestroyed;
            for (int i = 0; i < bushDestroyed.Length; i++)
            {   //Si le bush a �t� brul�
                if (bushDestroyed[i])
                {
                    bushes[i].SetActive(false);
                    terrainState.bushDestroyed[i] = true;
                }
                else
                {
                    terrainState.bushDestroyed[i] = false;
                }
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

        //dash
        gamestate.dashPicked = playerCollectibles.checkHasDash();

        //candle
        gamestate.candlePicked = playerCollectibles.checkHasCandle();

        //omamori
        gamestate.omamoriPicked = new bool[15];
        for(int i=0; i<gamestate.omamoriPicked.Length; i++)
        {
            gamestate.omamoriPicked[i] = playerCollectibles.omamoriPicked[i];
            Debug.Log("WRITTEN IN MEMORY : OMAMORI " + i + " " + gamestate.omamoriPicked[i]);
        }
        gamestate.darumaPicked = new bool[15];
        //daruma
        for (int i = 0; i < gamestate.darumaPicked.Length; i++)
        {
            gamestate.darumaPicked[i] = playerCollectibles.darumaPicked[i];
            Debug.Log("WRITTEN IN MEMORY : DARUMA " + i + " " + gamestate.darumaPicked[i]);

        }
        gamestate.scrollPicked = new bool[16];
        //scroll
        for (int i = 0; i < gamestate.scrollPicked.Length; i++)
        {
            gamestate.scrollPicked[i] = playerCollectibles.scrollPicked[i];
            Debug.Log("WRITTEN IN MEMORY : Scroll " + i + " " + gamestate.scrollPicked[i]);

        }

        gamestate.notePicked = new bool[5];
        //note
        for (int i = 0; i < gamestate.notePicked.Length; i++)
        {
            gamestate.notePicked[i] = playerCollectibles.notePicked[i];
            Debug.Log("WRITTEN IN MEMORY : Note " + i + " " + gamestate.notePicked[i]);

        }
        gamestate.lanternLightened = new bool[3];
        //lanterns
        for (int i = 0; i < gamestate.lanternLightened.Length; i++)
        {
            gamestate.lanternLightened[i] = playerCollectibles.lanternLightened[i];
            Debug.Log("WRITTEN IN MEMORY : Lantern " + i + " " + gamestate.lanternLightened[i]);

        }

        gamestate.wallDestroyed = new bool[12];
        //walls
        for (int i = 0; i < gamestate.wallDestroyed.Length; i++)
        {
            gamestate.wallDestroyed[i] = terrainState.wallDestroyed[i];
            Debug.Log("WRITTEN IN MEMORY : Wall " + i + " " + gamestate.wallDestroyed[i]);

        }

        gamestate.bushDestroyed = new bool[4];
        //bushes
        for (int i = 0; i < gamestate.bushDestroyed.Length; i++)
        {
            gamestate.bushDestroyed[i] = terrainState.bushDestroyed[i];
            Debug.Log("WRITTEN IN MEMORY : Bush " + i + " " + gamestate.bushDestroyed[i]);
            
        }



        PlayerPrefs.SetInt("JustDeleteSave", 0);
        return gamestate;
    }

    public void DisableSave(Gamestate gamestate)
    {
        // On d�r�f�rence le gamestate
        gamestate = null;
        PlayerPrefs.SetInt("JustDeleteSave", 0);
        PlayerPrefs.SetInt("Save Exists", 0);
    }
}