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


    public Gamestate()
    {

    }

    public void create(SaveLoadGamestate saveloadgamestate)
    {
        saveloadgamestate.createGamestate(this);
    }

   
}

