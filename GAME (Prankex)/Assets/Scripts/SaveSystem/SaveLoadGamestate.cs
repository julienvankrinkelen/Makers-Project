using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SaveLoadGamestate : MonoBehaviour
{

    public GameObject panelTransiDeath;
    public GameObject panelDeath;
    public Animator deathAnim;

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
    public int nbOmamori;

    public GameObject[] daruma;
    public bool[] darumaPicked = new bool[15];
    public int nbDaruma;

    //public GameObject[] scrolls;
    //public bool[] scrollPicked = new bool[12];
    public int nbCurrentScrolls;
    public int nbScrollsPicked;

    public GameObject candleItem;
    public bool candlePicked;

    public GameObject dashItem;
    public bool dashPicked;

    public GameObject[] notes;
    public bool[] notePicked = new bool[6];
    public int numberOfNotes;

    public bool[] lanternLightened = new bool[3];

    [Header("Terrain")]
    public TerrainState terrainState;
    public DoorScript doorScript;

    public GameObject[] walls;
    public bool[] wallDestroyed = new bool[12];

    public GameObject[] bushes;
    public bool[] bushDestroyed = new bool[4];

    public bool[] doorLights = new bool[3];

    [Header("Mobs")]
    public MobsState mobsState;

    public GameObject[] tanukis;
    public bool[] tanukiDied = new bool[12];

    public GameObject[] karakasas;
    public float[] karakasaLife = new float[11];

    public GameObject[] onibis;
    public float[] onibiLife = new float[7];

    public int JustLoadedScene;
    public int JustDeletedSave;

    private Gamestate gamestate;

    public void Start()
    {
        deathAnim = panelDeath.GetComponent<Animator>();
    }

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
        StartCoroutine(TempoLoadGamestate());

    }

    public IEnumerator TempoLoadGamestate()
    {
        //Tempo pour laisser le temps au jeu de charger tous les éléments, puis on les change
        yield return new WaitForSecondsRealtime(0.5f);
        //yield return new WaitForSecondsRealtime(0);
        JustLoadedScene = 0;
        PlayerPrefs.SetInt("JustLoadedScene", JustLoadedScene);
        int saveExists = PlayerPrefs.GetInt("Save Exists");
        if (saveExists == 1)
        {
            Gamestate data = SaveSystem.LoadGamestate();

            //player
            player.CurrentHealth = data.currentHealth;
            player.attackDamage = data.attackDamage;
            player.maxHealth = data.maxHealth;

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
            else
            {
                dashItem.SetActive(true);
                playerCollectibles.setDash(false);
            }

            //candle
            candlePicked = data.candlePicked;
            if (candlePicked)
            {
                candleItem.SetActive(false);
                playerCollectibles.setCandle(true);
            }
            else
            {
                candleItem.SetActive(true);
                playerCollectibles.setCandle(false);
            }

            //daruma
            darumaPicked = data.darumaPicked;
            int numberOfDaruma = 0;
            for (int i = 0; i < darumaPicked.Length; i++)
            {   //Si le daruma a été pick
                if (darumaPicked[i])
                {
                    daruma[i].SetActive(false);
                    //Ajoute le daruma au compteur, dans le cas où on aurait besoin du nombre
                    numberOfDaruma++;
                    playerCollectibles.darumaPicked[i] = true;
                }
                else
                {
                    daruma[i].SetActive(true);
                    playerCollectibles.darumaPicked[i] = false;
                }
                //playerCollectibles.setNumberDaruma(numberOfDaruma);
            }

            nbDaruma = data.nbDaruma;
            playerCollectibles.setNumberDaruma(nbDaruma);

            //omamori
            omamoriPicked = data.omamoriPicked;
            int numberOfOmamori = 0;
            for (int i=0; i<omamoriPicked.Length; i++)
            {   //Si l'omamori a été pick
                if (omamoriPicked[i])
                {
                    omamori[i].SetActive(false);
                    //Ajoute l'omamori au compteur, dans le cas où on aurait besoin du nombre
                    numberOfOmamori++;
                    playerCollectibles.omamoriPicked[i] = true;
                }
                else
                {
                    omamori[i].SetActive(true);
                    playerCollectibles.omamoriPicked[i] = false;
                }
              //  playerCollectibles.setNumberOmamori(numberOfOmamori);
            }

            nbOmamori = data.nbOmamori;
            playerCollectibles.setNumberOmamori(nbOmamori);

            //numberScrolls
            nbCurrentScrolls = data.nbCurrentScrolls;
            playerCollectibles.setNumberExplosiveScroll(nbCurrentScrolls);
            nbScrollsPicked = data.nbScrollsPicked;
            playerCollectibles.setNumberScrollsPicked(nbScrollsPicked);


            //notes
            notePicked = data.notePicked;
            for(int i = 0; i< notePicked.Length; i++)
            {   //Si la note a été pick
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
            numberOfNotes = data.numberOfNotes;
            playerCollectibles.setNumberOfNotes(numberOfNotes);

            //lanterns
            lanternLightened = data.lanternLightened;
            for (int i = 0; i < lanternLightened.Length; i++)
            {   //Si la lanterne a été allumée
                if (lanternLightened[i])
                {
                    playerCollectibles.setLanternLightened(i,true);
                    playerCollectibles.lanternLightened[i] = true;
                }
                else
                {
                    playerCollectibles.setLanternLightened(i, false);
                    playerCollectibles.lanternLightened[i] = false;
                }
            }


            //walls
            wallDestroyed = data.wallDestroyed;
            for (int i = 0; i < wallDestroyed.Length; i++)
            {   //Si le wall a été détruit
                if (wallDestroyed[i])
                {
                    walls[i].SetActive(false);
                    terrainState.wallDestroyed[i] = true;
                }
                else
                {
                    walls[i].SetActive(true);
                    terrainState.wallDestroyed[i] = false;
                }
            }

            //bushes
            bushDestroyed = data.bushDestroyed;
            for (int i = 0; i < bushDestroyed.Length; i++)
            {   //Si le bush a été brulé
                if (bushDestroyed[i])
                {
                    bushes[i].SetActive(false);
                    terrainState.bushDestroyed[i] = true;
                }
                else
                {
                    bushes[i].SetActive(true);
                    terrainState.bushDestroyed[i] = false;
                }
            }

            //door lights
            doorLights = data.doorLights;
            int nbDoorLights = 0;
            for (int i = 0; i < doorLights.Length; i++)
            {   //Si la light de la porte du boss a été allumée
                if (doorLights[i])
                {
                    terrainState.lightDoor(i);
                    nbDoorLights++;
                }
                else
                {
                    terrainState.unlightDoor(i);

                }
                doorScript.setCandleAnim(nbDoorLights);

            }


            //tanukis
            tanukiDied = data.tanukiDied;
            for(int i = 0; i < tanukiDied.Length; i++)
            {
                if (tanukiDied[i])
                {
                    //Désactiver le tanuki et son interaction etc.
                    tanukis[i].SetActive(false);
                    mobsState.tanukiDied[i] = true;
                    
                }
            }

            //karakasas
            karakasaLife = data.karakasaLife;
            for (int i = 0; i < karakasaLife.Length; i++)
            {
                mobsState.karakasaLife[i] = karakasaLife[i];
                //Récupère les scripts de chaque karakasas et set sa current health à sa vie du dernier point de sauvegarde
                if (karakasaLife[i] <= 0)
                {
                    karakasas[i].SetActive(false);
                }
                else 
                {
                    karakasas[i].GetComponent<EnemyScript>().currentHealth = karakasaLife[i];
                }
            }

            //onibis
            onibiLife = data.onibiLife;
            for (int i = 0; i < onibiLife.Length; i++) {

                //Pour la prochaine save, garder en mémoire celle d'avant ? vraiment utile séquentiellement ?
                mobsState.onibiLife[i] = onibiLife[i];
                //Récupère les scripts de chaque onibi et set sa current health à sa vie du dernier point de sauvegarde
                if (onibiLife[i] <= 0)
                {
                    onibis[i].SetActive(false);
                }
                else
                {
                    onibis[i].GetComponent<OnibiScript>().currentHealth = onibiLife[i];
                }
            }


            // Désactiver loading Screen
            deathAnim.SetBool("ShowLoadingScreen", false);
            panelTransiDeath.SetActive(false);

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
        gamestate.currentHealth = player.CurrentHealth;
        gamestate.attackDamage = player.attackDamage;
        gamestate.maxHealth = player.maxHealth;
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
        gamestate.nbOmamori = playerCollectibles.getOmamoriNumber();
        Debug.Log("WRITTEN IN MEMORY : Number of omamori : " + gamestate.nbOmamori);

        gamestate.darumaPicked = new bool[15];
        //daruma
        for (int i = 0; i < gamestate.darumaPicked.Length; i++)
        {
            gamestate.darumaPicked[i] = playerCollectibles.darumaPicked[i];
            Debug.Log("WRITTEN IN MEMORY : DARUMA " + i + " " + gamestate.darumaPicked[i]);
        }
        gamestate.nbDaruma = playerCollectibles.getDarumaNumber();
        Debug.Log("WRITTEN IN MEMORY : Number of daruma : " + gamestate.nbDaruma);

        /*
        gamestate.scrollPicked = new bool[12];
        //scroll
        for (int i = 0; i < gamestate.scrollPicked.Length; i++)
        {
            gamestate.scrollPicked[i] = playerCollectibles.scrollPicked[i];
            Debug.Log("WRITTEN IN MEMORY : Scroll " + i + " " + gamestate.scrollPicked[i]);

        }
        */
        gamestate.nbCurrentScrolls = playerCollectibles.getExplosiveScrollNumber();
        Debug.Log("WRITTEN IN MEMORY : Number of current scrolls : " + gamestate.nbCurrentScrolls);
        gamestate.nbScrollsPicked = playerCollectibles.getExplosiveScrollTotalNumber();
        Debug.Log("WRITTEN IN MEMORY : Number of scrolls total picked : " + gamestate.nbScrollsPicked);


        gamestate.notePicked = new bool[6];
        //note
        for (int i = 0; i < gamestate.notePicked.Length; i++)
        {
            gamestate.notePicked[i] = playerCollectibles.notePicked[i];
            Debug.Log("WRITTEN IN MEMORY : Note " + i + " " + gamestate.notePicked[i]);

        }
        gamestate.lanternLightened = new bool[3];
        gamestate.numberOfNotes = playerCollectibles.getNumberOfNotes();
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

        gamestate.doorLights = new bool[3];
        //door lights
        for (int i = 0; i < gamestate.doorLights.Length; i++)
        {
            gamestate.doorLights[i] = terrainState.doorLights[i];
            Debug.Log("WRITTEN IN MEMORY : door light " + i + " " + gamestate.doorLights[i]);

        }

        gamestate.tanukiDied = new bool[12];
        //tanukis
        for(int i = 0; i < gamestate.tanukiDied.Length; i++)
        {
            gamestate.tanukiDied[i] = mobsState.tanukiDied[i];
            Debug.Log("WRITTEN IN MEMORY : Tanuki " + i + " " + gamestate.tanukiDied[i]);
        }

        gamestate.karakasaLife = new float[11];
        //karakasas
        for (int i = 0; i < gamestate.karakasaLife.Length; i++)
        {
            gamestate.karakasaLife[i] = mobsState.karakasaLife[i];
            Debug.Log("WRITTEN IN MEMORY : Karakasa " + i + " life =  " + gamestate.karakasaLife[i]);
        }

        gamestate.onibiLife = new float[7];
        //onibis
        for (int i = 0; i < gamestate.onibiLife.Length; i++)
        {
            gamestate.onibiLife[i] = mobsState.onibiLife[i];
            Debug.Log("WRITTEN IN MEMORY : Onibi " + i + " life =  " + gamestate.onibiLife[i]);
        }



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