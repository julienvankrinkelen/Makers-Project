using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]



public class Gamestate
{
    //disable save
    public bool enableSave;
   
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

