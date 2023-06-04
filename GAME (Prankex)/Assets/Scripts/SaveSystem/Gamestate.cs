using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]



public class Gamestate
{
   
    //player
    public float currentHealth;
    public float attackDamage;
    public float[] positionPlayer;

    //ennemies
    public float[] positionEnnemy1;

    //colectibles
    public bool dashPicked;
    public bool candlePicked;

    public bool[] omamoriPicked;
    public int nbOmamori;

    public bool[] darumaPicked;
    public int nbDaruma;

    // public bool[] scrollPicked;
    public int nbCurrentScrolls;

    public bool[] notePicked;

    public bool[] lanternLightened;

    //Terrain
    public bool[] wallDestroyed;

    public bool[] bushDestroyed;

    public bool[] doorLights;

    //Mobs
    public bool[] tanukiDied;

    public float[] karakasaLife;

    public float[] onibiLife;





    public Gamestate()
    {

    }

    public void create(SaveLoadGamestate saveloadgamestate)
    {
        saveloadgamestate.createGamestate(this);
    }

   
}

