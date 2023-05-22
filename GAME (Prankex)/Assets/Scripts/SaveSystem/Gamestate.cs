using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]



public class Gamestate
{
   
    //player
    public float health;
    public float attackDamage;
    public float[] positionPlayer;

    //ennemies
    public float[] positionEnnemy1;

    //colectibles
    public bool dashPicked;
    public bool candlePicked;

    public bool[] omamoriPicked;

    public bool[] darumaPicked;

    public bool[] scrollPicked;

    public bool[] notePicked;

    public bool[] lanternLightened;

    //Terrain
    public bool[] wallDestroyed;

    public bool[] bushDestroyed;




    public Gamestate()
    {

    }

    public void create(SaveLoadGamestate saveloadgamestate)
    {
        saveloadgamestate.createGamestate(this);
    }

   
}

