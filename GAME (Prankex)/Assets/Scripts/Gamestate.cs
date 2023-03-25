using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]



public class Gamestate
{
   
    //player
    public int health;
    public int attackDamage;
    public float[] positionPlayer;

    //ennemies
    public float[] positionEnnemy1;
    public Gamestate()
    {

    }

    public void create(SaveLoadGamestate saveloadgamestate)
    {
        saveloadgamestate.createGamestate(this);
    }

   
}

