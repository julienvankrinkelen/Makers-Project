using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int health;
    public int attackDamage;
    public float[] position;
    

    public PlayerData(PlayerCombat player)
    {
        health = player.CurrentHealth;
        attackDamage = player.attackDamage;

        position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;

    }

   
}

